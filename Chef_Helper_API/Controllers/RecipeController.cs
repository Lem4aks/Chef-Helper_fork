using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Chef_Helper_API.Models;

namespace Chef_Helper_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RecipeController : Controller
    {
        public readonly ChefdbContext _dbContext;
        public RecipeController(ChefdbContext dbContext)
        {
            _dbContext = dbContext;
        }
        // GET: api/Recipe
        [HttpGet("/Recipes")]
        public IActionResult Get()
        {
            var recipes = _dbContext.Recipes.ToList();
            return Ok(recipes);
        }

        // GET: api/Recipe/name
        [HttpGet("{recipeName}")]
        public IActionResult Get(string recipeName)
        {
            var recipes = _dbContext.Recipes.Where(recipe => recipe.RecipeName.Contains(recipeName));

            if (recipes == null)
            {
                return NotFound();
            }
            return Ok(recipes);
        }

        // POST: api/Recipe
        [HttpPost]
        public IActionResult Post(Recipes recipe)
        {
            // Разделяем список ингредиентов, указанных в поле IngredientsNeeded, по запятой
            var ingredientsList = recipe.IngredientsNeeded?.Split(new[] { ", " }, StringSplitOptions.RemoveEmptyEntries);

            if (ingredientsList != null && ingredientsList.Length > 0)
            {
                // Приводим все названия ингредиентов к нижнему регистру
                var lowercaseIngredients = ingredientsList.Select(ingredient => ingredient.Split(' ')[1].Trim().ToLower());

                // Получаем все названия ингредиентов в таблице Warehouse в нижнем регистре
                var warehouseIngredients = _dbContext.Warehouse.Select(w => w.IngredientName.ToLower()).ToList();

                // Проверяем наличие каждого ингредиента в списке Warehouse
                foreach (var ingredient in lowercaseIngredients)
                {
                    if (!warehouseIngredients.Contains(ingredient))
                    {
                        // Ингредиент не найден в таблице Warehouse
                        return BadRequest($"Ингредиент '{ingredient}' отсутствует в таблице Warehouse.");
                    }
                }
            }
            else
            {
                // Список ингредиентов пуст или не указан
                return BadRequest("Список ингредиентов пуст или не указан.");
            }



            _dbContext.Recipes.Add(recipe);
            _dbContext.SaveChanges();
            return Ok();
        }

        // PUT: api/Recipe/5
        [HttpPut]
        public IActionResult Put(string name, Recipes updatedRecipe)
        {
            var recipeToUpdate = _dbContext.Recipes.Find(name);
            if (recipeToUpdate == null)
            {
                return NotFound();
            }

            recipeToUpdate.RecipeName = updatedRecipe.RecipeName;
            recipeToUpdate.IngredientsNeeded = updatedRecipe.IngredientsNeeded;
            recipeToUpdate.DishWeight = updatedRecipe.DishWeight;
            recipeToUpdate.CalorieValue = updatedRecipe.CalorieValue;

            _dbContext.SaveChanges();
            return Ok();
        }

        // DELETE: api/Recipe/5
        [HttpDelete]
        public IActionResult Delete(string name)
        {
            var recipeToDelete = _dbContext.Recipes.Find(name);
            if (recipeToDelete == null)
            {
                return NotFound();
            }

            _dbContext.Recipes.Remove(recipeToDelete);
            _dbContext.SaveChanges();
            return Ok();
        }
        [HttpGet("/AvailableRecipes")]
        public IActionResult GetRecipesWithAvailableIngredients()
        {
            // Получаем все рецепты
            List<Chef_Helper_API.Models.Recipes> recipes = _dbContext.Recipes.ToList();

            // Создаем словарь для хранения количества доступных ингредиентов на складе
            Dictionary<string, int> availableIngredients = new Dictionary<string, int>(StringComparer.OrdinalIgnoreCase);

            // Получаем все записи ингредиентов со склада
            List<Chef_Helper_API.Models.Warehouse> warehouseIngredients = _dbContext.Warehouse.ToList();

            foreach (var warehouseIngredient in warehouseIngredients)
            {
                string ingredientName = warehouseIngredient.IngredientName.Trim();
                int quantity = warehouseIngredient.WarehouseQuantity ?? 0;

                if (availableIngredients.ContainsKey(ingredientName))
                {
                    // Если ингредиент уже присутствует в словаре, добавляем количество
                    availableIngredients[ingredientName] += quantity;
                }
                else
                {
                    // Если ингредиент отсутствует в словаре, добавляем его с количеством
                    availableIngredients.Add(ingredientName, quantity);
                }
            }

            // Формируем список рецептов, которые можно приготовить
            List<RecipeWithQuantity> recipesWithAvailableIngredients = new List<RecipeWithQuantity>();

            foreach (var recipe in recipes)
            {
                string[] ingredientsList = recipe.IngredientsNeeded?.Split(new[] { ", " }, StringSplitOptions.RemoveEmptyEntries);

                if (ingredientsList != null && ingredientsList.Length > 0)
                {
                    bool canCookRecipe = true;

                    foreach (var ingredientItem in ingredientsList)
                    {
                        string[] parts = ingredientItem.Split(' ');

                        if (parts.Length != 2)
                        {
                            // Неверный формат записи ингредиента
                            canCookRecipe = false;
                            break;
                        }

                        string ingredientName = parts[1].Trim();
                        int requiredQuantity = int.Parse(parts[0]);

                        if (!availableIngredients.TryGetValue(ingredientName, out var availableQuantity) || requiredQuantity > availableQuantity)
                        {
                            // Ингредиент отсутствует на складе или его количество недостаточно
                            canCookRecipe = false;
                            break;
                        }
                    }

                    if (canCookRecipe)
                    {
                        // Рецепт можно приготовить, добавляем его в список с указанием доступного количества порций
                        recipesWithAvailableIngredients.Add(new RecipeWithQuantity
                        {
                            RecipeName = recipe.RecipeName,
                            AvailableQuantity = CalculateAvailableRecipeQuantity(ingredientsList, availableIngredients)
                        });
                    }
                }
            }

            return Ok(recipesWithAvailableIngredients);
        }

        // Метод для вычисления доступного количества порций рецепта
        private int CalculateAvailableRecipeQuantity(string[] ingredientsList, Dictionary<string, int> availableIngredients)
        {
            int availableQuantity = int.MaxValue;

            foreach (var ingredientItem in ingredientsList)
            {
                string[] parts = ingredientItem.Split(' ');
                string ingredientName = parts[1].Trim();
                int requiredQuantity = int.Parse(parts[0]);

                if (availableIngredients.TryGetValue(ingredientName, out var ingredientQuantity))
                {
                    int possibleQuantity = ingredientQuantity / requiredQuantity;
                    if (possibleQuantity < availableQuantity)
                    {
                        availableQuantity = possibleQuantity;
                    }
                }
            }

            return availableQuantity;
        }




    }
}
