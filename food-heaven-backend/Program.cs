using food_heaven_backend.Shared.Infrastructure.Persistence.EfCore.Configuration;
using food_heaven_backend.Shared.Domain.Repositories;
using food_heaven_backend.Shared.Infrastructure.Persistence.EfCore.Repositories;
using food_heaven_backend.Shared.Infrastructure.Persistence.EfCore.Seed;
using food_heaven_backend.PlanComidas.Domain.Services;
using food_heaven_backend.PlanComidas.Domain.Repositories;
using food_heaven_backend.PlanComidas.Application.Internal.CommandServices;
using food_heaven_backend.PlanComidas.Application.Internal.OutboundServices.Acl;
using food_heaven_backend.PlanComidas.Application.Internal.QueryServices;
using food_heaven_backend.PlanComidas.Infrastructure.Persistence.EfCore.Repositories;
using FluentValidation;
using food_heaven_backend.PlanComidas.Domain.Model.Commands;
using food_heaven_backend.PlanComidas.Application.Internal.Validators;
using food_heaven_backend.FoodCatalogContext.Application.Internal.CommandServices;
using food_heaven_backend.FoodCatalogContext.Application.Internal.QueryServices;
using food_heaven_backend.FoodCatalogContext.Domain.Repositories;
using food_heaven_backend.FoodCatalogContext.Domain.Model.Commands;
using food_heaven_backend.FoodCatalogContext.Application.Acl;
using food_heaven_backend.FoodCatalogContext.Application.Internal.Validators;
using food_heaven_backend.FoodCatalogContext.Domain.Services;
using food_heaven_backend.FoodCatalogContext.Infrastructure.Persistence.EfCore.Repositories;
using food_heaven_backend.FoodCatalogContext.Interfaces.Acl;
using food_heaven_backend.Security.Application.Acl;
using food_heaven_backend.Security.Application.Internal.CommandServices;
using food_heaven_backend.Security.Application.Internal.QueryServices;
using food_heaven_backend.Security.Domain.Repositories;
using food_heaven_backend.Security.Domain.Services;
using food_heaven_backend.Security.Infrastructure.Hashing;
using food_heaven_backend.Security.Infrastructure.Tokens;
using food_heaven_backend.Security.Infrastructure.Persistence.EfCore.Repositories;
using food_heaven_backend.Security.Interfaces.Acl;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

var authKey = builder.Configuration["Auth:key"] ?? throw new InvalidOperationException("Auth:key configuration is required.");

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
if (string.IsNullOrWhiteSpace(connectionString))
{
    throw new InvalidOperationException("DefaultConnection configuration is required.");
}

builder.Services.AddDbContext<FoodHeavenContext>(options =>
    options.UseSqlite(connectionString));

// Registro de controladores y Swagger
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Configuración de autenticación JWT
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = false,
            ValidateAudience = false,
            ValidateLifetime = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(authKey)),
            ClockSkew = TimeSpan.Zero // No permite desajustes de tiempo
        };
    });

// Registro de servicios de dominio y aplicación
builder.Services.AddScoped<IComidaCommandService, ComidaCommandServiceImpl>();
builder.Services.AddScoped<IComidaQueryService, ComidaQueryServiceImpl>();
builder.Services.AddScoped<IFoodCatalogContextFacade, FoodCatalogContextFacadeImpl>();
builder.Services.AddScoped<ISecurityContextFacade, SecurityContextFacadeImpl>();
builder.Services.AddScoped<IUserQueryService, UserQueryServiceImpl>();

// Registro de repositorios
builder.Services.AddScoped<IComidaRepository, ComidaRepository>();
builder.Services.AddScoped<IPlanComidaCommandService, PlanComidaCommandServiceImpl>();
builder.Services.AddScoped<IPlanComidaQueryService, PlanComidaQueryServiceImpl>();
builder.Services.AddScoped<ExternalFoodCatalogService>();
builder.Services.AddScoped<ExternalUserSubscriptionService>();
builder.Services.AddScoped<IPlanComidaRepository, PlanComidaRepository>();
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

// Registro de validadores
builder.Services.AddScoped<IValidator<CreateComidaCommand>, CreateComidaCommandValidator>();
builder.Services.AddScoped<IValidator<CreatePlanComidaCommand>, CreatePlanComidaCommandValidator>();

builder.Services.AddScoped<IUserCommandService, UserCommandServiceImpl>();
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IHashService, HashService>();
builder.Services.AddScoped<IJwtEncryptService, JwtEncryptService>();

// Configuración de CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowSpecificOrigin", policy =>
    {
        policy.WithOrigins(
            "http://localhost:5173",
            "http://127.0.0.1:5173"
        )
        .AllowAnyHeader()
        .AllowAnyMethod();
    });
});

// Build de la aplicación
var app = builder.Build();

// Middleware pipeline
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
    app.UseSwagger();
    app.UseSwaggerUI();
}

// Se asegura que la base de datos esté creada
// Intentar crear la base de datos solo si la conexión existe
try
{
    using var scope = app.Services.CreateScope();
    var context = scope.ServiceProvider.GetRequiredService<FoodHeavenContext>();
    if (!string.IsNullOrWhiteSpace(context.Database.GetConnectionString()))
    {
        await context.Database.EnsureCreatedAsync();
        await EnsureLocalUserProfileSchemaAsync(context);
        await EnsureLocalMealCatalogSchemaAsync(context);
        await FoodHeavenDataSeeder.SeedAsync(context);
    }
}
catch (Exception ex)
{
    Console.WriteLine($"[WARN] La base de datos no está disponible aún: {ex.Message}");
}


// Redirigir la raíz '/' a Swagger
app.Use(async (context, next) =>
{
    if (context.Request.Path == "/")
    {
        context.Response.Redirect("/swagger/index.html");
        return;
    }
    await next();
});

// Aplicar CORS
app.UseCors("AllowSpecificOrigin");

// Middleware de autenticación y autorización
app.UseAuthentication(); // Usa el esquema de autenticación configurado
app.UseAuthorization();

app.UseHttpsRedirection();

// Mapear los controladores
app.MapControllers();

app.Run();

static async Task EnsureLocalUserProfileSchemaAsync(FoodHeavenContext context)
{
    await context.Database.OpenConnectionAsync();

    try
    {
        await EnsureColumnAsync(context, "user", "full_name", "TEXT NOT NULL DEFAULT ''");
        await EnsureColumnAsync(context, "user", "address", "TEXT NOT NULL DEFAULT ''");
        await EnsureColumnAsync(context, "user", "delivery_addresses", "TEXT NOT NULL DEFAULT '[]'");
        await EnsureColumnAsync(context, "user", "payment_method", "TEXT NOT NULL DEFAULT ''");
        await EnsureColumnAsync(context, "user", "payment_card_brand", "TEXT NOT NULL DEFAULT ''");
        await EnsureColumnAsync(context, "user", "payment_card_last_four", "TEXT NOT NULL DEFAULT ''");
        await EnsureColumnAsync(context, "user", "payment_card_expiration", "TEXT NOT NULL DEFAULT ''");
        await EnsureColumnAsync(context, "Comida", "nombre_en", "TEXT NOT NULL DEFAULT ''");
        await EnsureColumnAsync(context, "Comida", "complemento_en", "TEXT NOT NULL DEFAULT ''");
    }
    finally
    {
        await context.Database.CloseConnectionAsync();
    }
}

static async Task EnsureLocalMealCatalogSchemaAsync(FoodHeavenContext context)
{
    await context.Database.OpenConnectionAsync();

    try
    {
        var legacySourceColumn = "id_" + "pro" + "veedor";
        var legacyFeatureColumn = "es_" + "especial";
        var hasLegacySourceColumn = await ColumnExistsAsync(context, "Comida", legacySourceColumn);
        var hasLegacyFeatureColumn = await ColumnExistsAsync(context, "Comida", legacyFeatureColumn);

        if (!hasLegacySourceColumn && !hasLegacyFeatureColumn) return;

        await context.Database.ExecuteSqlRawAsync("PRAGMA foreign_keys=OFF");
        await context.Database.ExecuteSqlRawAsync("DROP TABLE IF EXISTS \"Comida__new\"");
        await context.Database.ExecuteSqlRawAsync("""
            CREATE TABLE "Comida__new" (
                "id_comida" INTEGER NOT NULL CONSTRAINT "PK_Comida" PRIMARY KEY AUTOINCREMENT,
                "nombre" TEXT NOT NULL,
                "nombre_en" TEXT NOT NULL,
                "complemento" TEXT NOT NULL,
                "complemento_en" TEXT NOT NULL,
                "url" TEXT NOT NULL,
                "cal" INTEGER NOT NULL,
                "prote" INTEGER NOT NULL,
                "carbo" INTEGER NOT NULL,
                "grasa" INTEGER NOT NULL,
                "id_tipo_comida" INTEGER NOT NULL,
                CONSTRAINT "FK_Comida_TipoComida_id_tipo_comida" FOREIGN KEY ("id_tipo_comida") REFERENCES "TipoComida" ("id_tipo_comida") ON DELETE RESTRICT
            )
            """);
        await context.Database.ExecuteSqlRawAsync("""
            INSERT INTO "Comida__new" (
                "id_comida",
                "nombre",
                "nombre_en",
                "complemento",
                "complemento_en",
                "url",
                "cal",
                "prote",
                "carbo",
                "grasa",
                "id_tipo_comida"
            )
            SELECT
                "id_comida",
                "nombre",
                "nombre_en",
                "complemento",
                "complemento_en",
                "url",
                "cal",
                "prote",
                "carbo",
                "grasa",
                "id_tipo_comida"
            FROM "Comida"
            """);
        await context.Database.ExecuteSqlRawAsync("DROP TABLE \"Comida\"");
        await context.Database.ExecuteSqlRawAsync("ALTER TABLE \"Comida__new\" RENAME TO \"Comida\"");
        await context.Database.ExecuteSqlRawAsync("CREATE INDEX IF NOT EXISTS \"IX_Comida_id_tipo_comida\" ON \"Comida\" (\"id_tipo_comida\")");
        await context.Database.ExecuteSqlRawAsync("PRAGMA foreign_keys=ON");
    }
    finally
    {
        await context.Database.CloseConnectionAsync();
    }
}

static async Task EnsureColumnAsync(FoodHeavenContext context, string tableName, string columnName, string definition)
{
    var hasColumn = await ColumnExistsAsync(context, tableName, columnName);

    if (!hasColumn)
    {
        var sql = "ALTER TABLE \"" + tableName + "\" ADD COLUMN " + columnName + " " + definition;
        await context.Database.ExecuteSqlRawAsync(sql);
    }
}

static async Task<bool> ColumnExistsAsync(FoodHeavenContext context, string tableName, string columnName)
{
    await using var command = context.Database.GetDbConnection().CreateCommand();
    command.CommandText = $"PRAGMA table_info('{tableName}')";

    await using (var reader = await command.ExecuteReaderAsync())
    {
        while (await reader.ReadAsync())
        {
            if (string.Equals(reader.GetString(1), columnName, StringComparison.OrdinalIgnoreCase))
            {
                return true;
            }
        }
    }

    return false;
}
