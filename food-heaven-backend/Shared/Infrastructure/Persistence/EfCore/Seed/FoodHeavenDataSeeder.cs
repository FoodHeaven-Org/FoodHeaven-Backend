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
            CreateMeal(1, "Sandwich mixto con jugo", "Pan tostado con jamon, queso, cafe y jugo", "/desayuno1.png", 430, 21, 52, 15, 1),
            CreateMeal(2, "Croissant de jamon y queso", "Croissant relleno con jamon y queso", "/desayuno2.jpg", 410, 17, 38, 21, 1),
            CreateMeal(3, "Pan con chicharron y tamal", "Cerdo, camote, zarza criolla, tamal y cafe", "/desayuno3.png", 690, 34, 76, 28, 1),
            CreateMeal(4, "Pan con asado y jugo", "Pan con carne, papas y jugo de fresa", "/desayuno4%20(3).jpg", 580, 30, 62, 22, 1),
            CreateMeal(5, "Arroz con pollo", "Pollo, arroz verde y salsa criolla", "/almuerzo1%20(2).jpg", 720, 42, 82, 24, 2),
            CreateMeal(6, "Lomo saltado clasico", "Carne salteada, arroz y papas doradas", "/almuerzo2%20(2).jpg", 760, 40, 88, 28, 2),
            CreateMeal(7, "Aji de gallina", "Pollo en crema de aji amarillo con arroz", "/almuerzo3%20(2).jpg", 680, 33, 72, 27, 2),
            CreateMeal(8, "Seco de res con frejoles", "Res guisada, frejoles, arroz y papa", "/almuerzo4%20(2).jpg", 790, 45, 90, 26, 2),
            CreateMeal(9, "Ensalada de quinoa", "Quinoa, pepino, tomate y hierbas frescas", "/cena1%20(2).jpg", 430, 14, 54, 16, 3),
            CreateMeal(10, "Sopa criolla con carne", "Caldo de tomate, fideos, carne y huevo", "/cena2%20(2).png", 520, 29, 50, 21, 3),
            CreateMeal(11, "Filete de pescado", "Pescado a la plancha con ensalada", "/cena3%20(2).jpg", 390, 32, 18, 16, 3),
            CreateMeal(12, "Tortilla de quinua", "Tortilla de quinua con ensalada criolla", "/cena4.jpg", 460, 24, 46, 18, 3)
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
