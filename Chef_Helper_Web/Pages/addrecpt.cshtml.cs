using Chef_Helper_API.Models;
using Chef_Helper_Web.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Chef_Helper_Web.Pages
{
    public class addrecptModel : PageModel
    {
        private readonly IRecipesService _recipeService;
        public addrecptModel(IRecipesService recipeService)
        {
            _recipeService = recipeService;
        }

        public IActionResult OnPost()
        {
            string recipeName = Request.Form["RecipeName"];
            string calories = Request.Form["CalorieValue"];
            string ingredients = Request.Form["IngredientsNeeded"];
            string portionWeight = Request.Form["DishWeight"];

            // Создайте объект Recipe и вызовите метод сервиса для добавления рецепта
            Chef_Helper_Web.Services.Recipes recipe = new Chef_Helper_Web.Services.Recipes
            {
                RecipeName = recipeName,
                CalorieValue = calories,
                IngredientsNeeded = ingredients,
                DishWeight = portionWeight
            };

            _recipeService.Post(recipe);

            return RedirectToPage("/Index");
        }
        private readonly ILogger<addrecptModel> _logger;

        public addrecptModel(ILogger<addrecptModel> logger)
        {
            _logger = logger;
        }

        public void OnGet()
        {
        }
  
    }
}