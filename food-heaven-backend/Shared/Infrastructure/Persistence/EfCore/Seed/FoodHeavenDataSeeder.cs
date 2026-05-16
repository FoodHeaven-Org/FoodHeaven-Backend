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
        var meals = new[]
        {
            CreateMeal(1, "Avena con frutas", "Avena, platano y fresas", "/desayuno1.png", 360, 13, 58, 8, 1),
            CreateMeal(2, "Tostada con palta y huevo", "Pan integral, palta y huevo", "/desayuno2.jpg", 420, 22, 40, 18, 1),
            CreateMeal(3, "Bowl de yogurt", "Yogurt, granola y frutos rojos", "/desayuno3.png", 390, 24, 46, 10, 1),
            CreateMeal(4, "Omelette de verduras", "Huevos, verduras salteadas y queso fresco", "/desayuno4%20(3).jpg", 410, 28, 18, 24, 1),
            CreateMeal(5, "Pollo con quinoa", "Pechuga de pollo, quinoa y ensalada", "/almuerzo1%20(2).jpg", 620, 45, 58, 18, 2),
            CreateMeal(6, "Lomo saltado balanceado", "Carne magra, papa al horno y arroz integral", "/almuerzo2%20(2).jpg", 680, 42, 72, 22, 2),
            CreateMeal(7, "Bowl vegetariano", "Garbanzos, camote, verduras y tahini", "/almuerzo3%20(2).jpg", 560, 24, 76, 16, 2),
            CreateMeal(8, "Pasta integral con pollo", "Pasta integral, pollo y verduras", "/almuerzo4%20(2).jpg", 640, 38, 82, 14, 2),
            CreateMeal(9, "Salmon con verduras", "Salmon, brocoli y arroz jazmin", "/cena1%20(2).jpg", 590, 38, 44, 24, 3),
            CreateMeal(10, "Wrap de pavo", "Tortilla integral, pavo y vegetales", "/cena2%20(2).png", 430, 31, 42, 13, 3),
            CreateMeal(11, "Crema de verduras", "Crema ligera de verduras y tostadas integrales", "/cena3%20(2).jpg", 350, 12, 48, 9, 3),
            CreateMeal(12, "Ensalada de pollo", "Pollo, hojas verdes, palta y semillas", "/cena4.jpg", 460, 36, 30, 20, 3)
        };

        foreach (var meal in meals)
        {
            var existingMeal = await context.Comidas.FindAsync(meal.Id);
            if (existingMeal == null)
            {
                context.Comidas.Add(meal);
                continue;
            }

            existingMeal.Nombre = meal.Nombre;
            existingMeal.Complemento = meal.Complemento;
            existingMeal.Url = meal.Url;
            existingMeal.Cal = meal.Cal;
            existingMeal.Prote = meal.Prote;
            existingMeal.Carbo = meal.Carbo;
            existingMeal.Grasa = meal.Grasa;
            existingMeal.Id_Proveedor = meal.Id_Proveedor;
            existingMeal.id_tipo_comida = meal.id_tipo_comida;
            existingMeal.es_especial = meal.es_especial;
        }

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
