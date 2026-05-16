using food_heaven_backend.Shared.Infraestructure.Persistence.Configuration;
using food_heaven_backend.Shared.Domain.Repositories;
using food_heaven_backend.Shared.Infraestructure.Persistence.Repositories;
using food_heaven_backend.PlanComidas.Domain.Services;
using food_heaven_backend.PlanComidas.Application.CommandServices;
using food_heaven_backend.PlanComidas.Application.QueryServices;
using food_heaven_backend.PlanComidas.Infrastructure;
using FluentValidation;
using food_heaven_backend.PlanComidas.Domain.Models.Commands;
using food_heaven_backend.PlanComidas.Domain.Models.Validators;
using food_heaven_backend.FoodCatalogContext.Application.CommandServices;
using food_heaven_backend.FoodCatalogContext.Application.QueryServices;
using food_heaven_backend.FoodCatalogContext.Domain;
using food_heaven_backend.FoodCatalogContext.Domain.Models.Commands;
using food_heaven_backend.FoodCatalogContext.Domain.Models.Validators;
using food_heaven_backend.FoodCatalogContext.Domain.Services;
using food_heaven_backend.FoodCatalogContext.Infraestructure;
using food_heaven_backend.Security.Application;
using food_heaven_backend.Security.Domain.Repositories;
using food_heaven_backend.Security.Domain.Service;
using food_heaven_backend.Security.Infraestrucutre;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Obtener cadena de conexión
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

// Configuración del contexto de base de datos con MySQL
builder.Services.AddDbContext<FoodHeavenContext>(options =>
    options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString)));

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
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Auth:key"])),
            ClockSkew = TimeSpan.Zero // No permite desajustes de tiempo
        };
    });

// Registro de servicios de dominio y aplicación
builder.Services.AddScoped<IProveedorCommandService, ProveedorCommandService>();
builder.Services.AddScoped<IProveedorQueryService, ProveedorQueryService>();
builder.Services.AddScoped<IComidaCommandService, ComidaCommandService>();
builder.Services.AddScoped<IComidaQueryService, ComidaQueryService>();
builder.Services.AddScoped<IUserQueryService, UserQueryService>();

// Registro de repositorios
builder.Services.AddScoped<IProveedorRepository, ProveedorRepository>();
builder.Services.AddScoped<IComidaRepository, ComidaRepository>();
builder.Services.AddScoped<IPlanComidaCommandService, PlanComidaCommandService>();
builder.Services.AddScoped<IPlanComidaQueryService, PlanComidaQueryService>();
builder.Services.AddScoped<IPlanComidaRepository, PlanComidaRepository>();
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

// Registro de validadores
builder.Services.AddScoped<IValidator<CreateProveedorCommand>, CreateProveedorCommandValidator>();
builder.Services.AddScoped<IValidator<CreateComidaCommand>, CreateComidaCommandValidator>();
builder.Services.AddScoped<IValidator<CreatePlanComidaCommand>, CreatePlanComidaCommandValidator>();

builder.Services.AddScoped<IUserCommandService, UserCommandService>();
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
            "https://food-heaven.onrender.com" // o la URL final del frontend si la tienes
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
        context.Database.EnsureCreated();
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
