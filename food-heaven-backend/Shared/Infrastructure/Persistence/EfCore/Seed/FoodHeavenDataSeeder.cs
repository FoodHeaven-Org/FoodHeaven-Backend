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

    private static async Task SeedMealsAsync(FoodHeavenContext context)
    {
        var meals = new[]
        {
            CreateMeal(1, "Sandwich mixto con jugo", "Mixed sandwich with juice", "Pan tostado con jamon, queso, cafe y jugo", "Toast with ham, cheese, coffee, and juice", "/desayuno1.png", 430, 21, 52, 15, 1),
            CreateMeal(2, "Croissant de jamon y queso", "Ham and cheese croissant", "Croissant relleno con jamon y queso", "Croissant filled with ham and cheese", "/desayuno2.jpg", 410, 17, 38, 21, 1),
            CreateMeal(3, "Pan con chicharron y tamal", "Pork sandwich with tamal", "Cerdo, camote, zarza criolla, tamal y cafe", "Pork, sweet potato, onion relish, tamal, and coffee", "/desayuno3.png", 690, 34, 76, 28, 1),
            CreateMeal(4, "Pan con asado y jugo", "Roast beef sandwich with juice", "Pan con carne, papas y jugo de fresa", "Beef sandwich, potatoes, and strawberry juice", "/desayuno4%20(3).jpg", 580, 30, 62, 22, 1),
            CreateMeal(5, "Arroz con pollo", "Chicken cilantro rice", "Pollo, arroz verde y salsa criolla", "Chicken, cilantro rice, and onion relish", "/almuerzo1%20(2).jpg", 720, 42, 82, 24, 2),
            CreateMeal(6, "Lomo saltado clasico", "Classic lomo saltado", "Carne salteada, arroz y papas doradas", "Sauteed beef, rice, and golden potatoes", "/almuerzo2%20(2).jpg", 760, 40, 88, 28, 2),
            CreateMeal(7, "Aji de gallina", "Creamy chicken chili stew", "Pollo en crema de aji amarillo con arroz", "Chicken in yellow chili cream with rice", "/almuerzo3%20(2).jpg", 680, 33, 72, 27, 2),
            CreateMeal(8, "Seco de res con frejoles", "Beef stew with beans", "Res guisada, frejoles, arroz y papa", "Braised beef, beans, rice, and potato", "/almuerzo4%20(2).jpg", 790, 45, 90, 26, 2),
            CreateMeal(9, "Ensalada de quinoa", "Quinoa salad", "Quinoa, pepino, tomate y hierbas frescas", "Quinoa, cucumber, tomato, and fresh herbs", "/cena1%20(2).jpg", 430, 14, 54, 16, 3),
            CreateMeal(10, "Sopa criolla con carne", "Creole beef soup", "Caldo de tomate, fideos, carne y huevo", "Tomato broth, noodles, beef, and egg", "/cena2%20(2).png", 520, 29, 50, 21, 3),
            CreateMeal(11, "Filete de pescado", "Grilled fish fillet", "Pescado a la plancha con ensalada", "Grilled fish with salad", "/cena3%20(2).jpg", 390, 32, 18, 16, 3),
            CreateMeal(12, "Tortilla de quinua", "Quinoa omelette", "Tortilla de quinua con ensalada criolla", "Quinoa omelette with onion salad", "/cena4.jpg", 460, 24, 46, 18, 3)
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
            existingMeal.NombreEn = meal.NombreEn;
            existingMeal.Complemento = meal.Complemento;
            existingMeal.ComplementoEn = meal.ComplementoEn;
            existingMeal.Url = meal.Url;
            existingMeal.Cal = meal.Cal;
            existingMeal.Prote = meal.Prote;
            existingMeal.Carbo = meal.Carbo;
            existingMeal.Grasa = meal.Grasa;
            existingMeal.CatalogSourceId = meal.CatalogSourceId;
            existingMeal.id_tipo_comida = meal.id_tipo_comida;
        }

        await context.SaveChangesAsync();
    }

    private static Comida CreateMeal(
        int id,
        string name,
        string englishName,
        string complement,
        string englishComplement,
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
            NombreEn = englishName,
            Complemento = complement,
            ComplementoEn = englishComplement,
            Url = imageUrl,
            Cal = calories,
            Prote = protein,
            Carbo = carbs,
            Grasa = fat,
            CatalogSourceId = 1,
            id_tipo_comida = mealTypeId
        };
    }
}
