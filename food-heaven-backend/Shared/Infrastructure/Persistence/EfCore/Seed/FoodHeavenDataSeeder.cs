using food_heaven_backend.FoodCatalogContext.Domain.Model.Entities;
using food_heaven_backend.FoodCatalogContext.Domain.Model.ValueObjects;
using food_heaven_backend.Shared.Infrastructure.Persistence.EfCore.Configuration;
using Microsoft.EntityFrameworkCore;

namespace food_heaven_backend.Shared.Infrastructure.Persistence.EfCore.Seed;

public static class FoodHeavenDataSeeder
{
    public static async Task SeedAsync(FoodHeavenContext context)
    {
        await SeedMealTypesAsync(context);
        await SeedProviderTypesAsync(context);
        await SeedProvidersAsync(context);
        await SeedMealsAsync(context);
    }

    private static async Task SeedMealTypesAsync(FoodHeavenContext context)
    {
        if (await context.TipoComidas.AnyAsync()) return;

        context.TipoComidas.AddRange(
            new TipoComida { Id = 1, Descripcion = TipoComidas.Desayuno },
            new TipoComida { Id = 2, Descripcion = TipoComidas.Almuerzo },
            new TipoComida { Id = 3, Descripcion = TipoComidas.Cena }
        );

        await context.SaveChangesAsync();
    }

    private static async Task SeedProviderTypesAsync(FoodHeavenContext context)
    {
        if (await context.TiposProveedor.AnyAsync()) return;

        context.TiposProveedor.Add(new TipoProveedor
        {
            Id = 1,
            Descripcion = TipoProveedorDescripcion.ProveedorDirecto
        });

        await context.SaveChangesAsync();
    }

    private static async Task SeedProvidersAsync(FoodHeavenContext context)
    {
        if (await context.Proveedores.AnyAsync()) return;

        context.Proveedores.Add(new Proveedor
        {
            Id = 1,
            Nombre = "FoodHeaven Kitchen",
            Distrito = "Lima",
            Contacto = "local@foodheaven.test",
            TipoProveedorId = 1
        });

        await context.SaveChangesAsync();
    }

    private static async Task SeedMealsAsync(FoodHeavenContext context)
    {
        if (await context.Comidas.AnyAsync()) return;

        context.Comidas.AddRange(
            CreateMeal(1, "Avena con frutas", "Avena, platano y fresas", "https://images.unsplash.com/photo-1517673132405-a56a62b18caf?auto=format&fit=crop&w=900&q=80", 360, 13, 58, 8, 1),
            CreateMeal(2, "Pan integral con huevo", "Pan integral, huevo y palta", "https://images.unsplash.com/photo-1525351484163-7529414344d8?auto=format&fit=crop&w=900&q=80", 420, 22, 40, 18, 1),
            CreateMeal(3, "Yogurt protein bowl", "Yogurt, granola y arandanos", "https://images.unsplash.com/photo-1511690743698-d9d85f2fbf38?auto=format&fit=crop&w=900&q=80", 390, 24, 46, 10, 1),
            CreateMeal(4, "Pollo con quinoa", "Pechuga de pollo, quinoa y ensalada", "https://images.unsplash.com/photo-1546069901-ba9599a7e63c?auto=format&fit=crop&w=900&q=80", 620, 45, 58, 18, 2),
            CreateMeal(5, "Lomo saltado balanceado", "Carne magra, papa al horno y arroz integral", "https://images.unsplash.com/photo-1544025162-d76694265947?auto=format&fit=crop&w=900&q=80", 680, 42, 72, 22, 2),
            CreateMeal(6, "Bowl vegetariano", "Garbanzos, camote, verduras y tahini", "https://images.unsplash.com/photo-1512621776951-a57141f2eefd?auto=format&fit=crop&w=900&q=80", 560, 24, 76, 16, 2),
            CreateMeal(7, "Salmon con verduras", "Salmon, brocoli y arroz jazmin", "https://images.unsplash.com/photo-1467003909585-2f8a72700288?auto=format&fit=crop&w=900&q=80", 590, 38, 44, 24, 3),
            CreateMeal(8, "Wrap de pavo", "Tortilla integral, pavo y vegetales", "https://images.unsplash.com/photo-1565299585323-38d6b0865b47?auto=format&fit=crop&w=900&q=80", 430, 31, 42, 13, 3),
            CreateMeal(9, "Crema de verduras", "Crema ligera de verduras y tostadas integrales", "https://images.unsplash.com/photo-1547592180-85f173990554?auto=format&fit=crop&w=900&q=80", 350, 12, 48, 9, 3)
        );

        await context.SaveChangesAsync();
    }

    private static Comida CreateMeal(
        int id,
        string name,
        string complement,
        string imageUrl,
        int calories,
        int protein,
        int carbs,
        int fat,
        int mealTypeId)
    {
        return new Comida
        {
            Id = id,
            Nombre = name,
            Complemento = complement,
            Url = imageUrl,
            Cal = calories,
            Prote = protein,
            Carbo = carbs,
            Grasa = fat,
            Id_Proveedor = 1,
            id_tipo_comida = mealTypeId,
            es_especial = 0
        };
    }
}
