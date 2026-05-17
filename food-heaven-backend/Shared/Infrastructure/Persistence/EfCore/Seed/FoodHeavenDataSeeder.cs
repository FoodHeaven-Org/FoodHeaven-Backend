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
            CreateMeal(1, "Sandwich mixto con jugo", "Mixed sandwich with juice", "Pan tostado con jamon, queso, cafe y jugo", "Toast with ham, cheese, coffee, and juice", "https://images.unsplash.com/photo-1528735602780-2552fd46c7af?auto=format&fit=crop&w=900&q=80", 430, 21, 52, 15, 1),
            CreateMeal(2, "Croissant de jamon y queso", "Ham and cheese croissant", "Croissant relleno con jamon y queso", "Croissant filled with ham and cheese", "https://images.unsplash.com/photo-1555507036-ab1f4038808a?auto=format&fit=crop&w=900&q=80", 410, 17, 38, 21, 1),
            CreateMeal(3, "Pan con chicharron y tamal", "Pork sandwich with tamal", "Cerdo, camote, zarza criolla, tamal y cafe", "Pork, sweet potato, onion relish, tamal, and coffee", "https://upload.wikimedia.org/wikipedia/commons/f/f0/Peruvian_breakfast.jpg", 690, 34, 76, 28, 1),
            CreateMeal(4, "Butifarra peruana", "Peruvian butifarra sandwich", "Jamon del pais, zarza criolla y pan frances", "Country ham, onion relish, and French bread", "https://upload.wikimedia.org/wikipedia/commons/4/4b/Butifarra_del_Per%C3%BA.jpg", 520, 28, 54, 19, 1),
            CreateMeal(5, "Arroz con pollo", "Peruvian chicken rice", "Pollo, arroz verde, arvejas y salsa criolla", "Chicken, cilantro rice, peas, and onion relish", "https://upload.wikimedia.org/wikipedia/commons/e/e6/Arroz_con_pollo_peruano.jpg", 690, 38, 82, 22, 2),
            CreateMeal(6, "Lomo saltado clasico", "Classic lomo saltado", "Carne salteada, arroz y papas doradas", "Sauteed beef, rice, and golden potatoes", "https://upload.wikimedia.org/wikipedia/commons/c/ca/Lomo-saltado-perudelights.jpg", 760, 40, 88, 28, 2),
            CreateMeal(7, "Aji de gallina", "Creamy chicken chili stew", "Pollo en crema de aji amarillo con arroz", "Chicken in yellow chili cream with rice", "https://upload.wikimedia.org/wikipedia/commons/5/56/Aj%C3%AD_De_Gallina.jpg", 680, 33, 72, 27, 2),
            CreateMeal(8, "Seco de res con frejoles", "Beef stew with beans", "Res guisada, frejoles, arroz y salsa criolla", "Braised beef, beans, rice, and onion relish", "https://upload.wikimedia.org/wikipedia/commons/0/04/Seco_de_carne_peru.jpg", 790, 45, 90, 26, 2),
            CreateMeal(9, "Ensalada de quinoa", "Quinoa salad", "Quinoa, pepino, tomate y hierbas frescas", "Quinoa, cucumber, tomato, and fresh herbs", "https://images.unsplash.com/photo-1505253716362-afaea1d3d1af?auto=format&fit=crop&w=900&q=80", 430, 14, 54, 16, 3),
            CreateMeal(10, "Cazuela peruana", "Peruvian cazuela soup", "Caldo de res con verduras, choclo y pasta", "Beef broth with vegetables, corn, and pasta", "https://upload.wikimedia.org/wikipedia/commons/d/d5/Cazuela_Peru.jpg", 430, 26, 48, 12, 3),
            CreateMeal(11, "Filete de pescado", "Grilled fish fillet", "Pescado a la plancha con ensalada", "Grilled fish with salad", "https://images.unsplash.com/photo-1519708227418-c8fd9a32b7a2?auto=format&fit=crop&w=900&q=80", 390, 32, 18, 16, 3),
            CreateMeal(12, "Tortilla de quinua", "Quinoa omelette", "Tortilla de quinua con ensalada criolla", "Quinoa omelette with onion salad", "https://images.unsplash.com/photo-1510693206972-df098062cb71?auto=format&fit=crop&w=900&q=80", 460, 24, 46, 18, 3),
            CreateMeal(101, "Avena con frutos rojos", "Oatmeal with berries", "Avena, leche, fresas, arandanos y nueces", "Oats, milk, strawberries, blueberries, and walnuts", "https://images.unsplash.com/photo-1517673132405-a56a62b18caf?auto=format&fit=crop&w=900&q=80", 380, 14, 58, 11, 1),
            CreateMeal(102, "Tostada con palta y huevo", "Avocado egg toast", "Pan integral, palta, huevo y tomate", "Whole-grain toast, avocado, egg, and tomato", "https://images.unsplash.com/photo-1525351484163-7529414344d8?auto=format&fit=crop&w=900&q=80", 450, 20, 42, 22, 1),
            CreateMeal(103, "Yogurt con granola", "Yogurt with granola", "Yogurt natural, granola, miel y fruta fresca", "Plain yogurt, granola, honey, and fresh fruit", "https://images.unsplash.com/photo-1488477181946-6428a0291777?auto=format&fit=crop&w=900&q=80", 360, 16, 50, 10, 1),
            CreateMeal(104, "Omelette de verduras", "Vegetable omelette", "Huevos, espinaca, champinones y queso fresco", "Eggs, spinach, mushrooms, and fresh cheese", "https://images.unsplash.com/photo-1510693206972-df098062cb71?auto=format&fit=crop&w=900&q=80", 410, 26, 12, 27, 1),
            CreateMeal(105, "Pollo a la plancha con quinua", "Grilled chicken with quinoa", "Pechuga de pollo, quinua y verduras salteadas", "Chicken breast, quinoa, and sauteed vegetables", "https://images.unsplash.com/photo-1532550907401-a500c9a57435?auto=format&fit=crop&w=900&q=80", 640, 48, 58, 19, 2),
            CreateMeal(106, "Estofado de pollo", "Chicken stew", "Pollo guisado con arroz integral y papa", "Chicken stew with brown rice and potato", "https://upload.wikimedia.org/wikipedia/commons/3/35/White_rice_with_chicken_stew.jpg", 610, 38, 66, 20, 2),
            CreateMeal(107, "Pasta integral con pollo", "Whole-grain chicken pasta", "Pasta integral, pollo, tomate y albahaca", "Whole-grain pasta, chicken, tomato, and basil", "https://images.unsplash.com/photo-1551183053-bf91a1d81141?auto=format&fit=crop&w=900&q=80", 690, 42, 78, 21, 2),
            CreateMeal(108, "Bowl vegetariano", "Vegetarian bowl", "Garbanzos, camote, arroz integral y verduras", "Chickpeas, sweet potato, brown rice, and vegetables", "https://images.unsplash.com/photo-1512621776951-a57141f2eefd?auto=format&fit=crop&w=900&q=80", 560, 22, 82, 15, 2),
            CreateMeal(109, "Crema de verduras", "Vegetable cream soup", "Crema ligera de zapallo, zanahoria y tostadas", "Light squash and carrot soup with toast", "https://images.unsplash.com/photo-1547592166-23ac45744acd?auto=format&fit=crop&w=900&q=80", 330, 10, 46, 12, 3),
            CreateMeal(110, "Wrap de pavo", "Turkey wrap", "Tortilla integral, pavo, lechuga y palta", "Whole-grain tortilla, turkey, lettuce, and avocado", "https://images.unsplash.com/photo-1626700051175-6818013e1d4f?auto=format&fit=crop&w=900&q=80", 470, 32, 38, 20, 3),
            CreateMeal(111, "Salmon con verduras", "Salmon with vegetables", "Salmon al horno, brocoli y arroz jazmin", "Baked salmon, broccoli, and jasmine rice", "https://images.unsplash.com/photo-1467003909585-2f8a72700288?auto=format&fit=crop&w=900&q=80", 590, 39, 44, 28, 3),
            CreateMeal(112, "Ensalada de pollo", "Chicken salad", "Pollo, hojas verdes, palta y semillas", "Chicken, greens, avocado, and seeds", "https://images.unsplash.com/photo-1540420773420-3366772f4999?auto=format&fit=crop&w=900&q=80", 460, 35, 28, 22, 3)
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
            id_tipo_comida = mealTypeId,
        };
    }
}
