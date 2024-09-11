using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using System.Net.Http;
using System.Threading.Tasks;
using Chef_Helper_API;
using Microsoft.AspNetCore.Components;

namespace Chef_Helper_Web.Services
{
    public class Recipes
    {
        public string RecipeName { get; set; } = null!;

        public string? IngredientsNeeded { get; set; }

        public string? DishWeight { get; set; }

        public string? CalorieValue { get; set; }
    }
    public class RecipesService : IRecipesService
    {
        private readonly ChefdbContext _dbContext;
        private readonly HttpClient _httpClient;


        public RecipesService(HttpClient httpClient)
        {
            _httpClient = httpClient;
            _httpClient.BaseAddress = new Uri("https://localhost:7114/");
        }
        public async Task<List<Recipes>> GetRecipes()
        {
            return await _httpClient.GetFromJsonAsync<List<Recipes>>("/Recipes");
        }
        public async Task<Recipes> GetRecipes(string name)
        {
            return await _httpClient.GetFromJsonAsync<Recipes>($"api/Recipe/{name}");
        }

        public async Task<Recipes> Update (string recipeToUpdate, Recipes updatedRecipe)
        {
            await _httpClient.PutAsJsonAsync($"api/Recipe/{recipeToUpdate}", updatedRecipe);
            return updatedRecipe;
        }

        public async Task<Recipes> Post(Recipes recipe)
        {
            var response = await _httpClient.PostAsJsonAsync("api/Recipe", recipe);


            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadFromJsonAsync<Recipes>();
            }
            else
            {
                throw new Exception("Failed to post the recipe. HTTP response status: " + response.StatusCode);
            }
        }

        public async Task<List<RecipeWithQuantity>> GetRecipesAvailable()
        {
            return await _httpClient.GetFromJsonAsync<List<RecipeWithQuantity>>($"/AvailableRecipes");
        }

        public async Task Delete(string recipeName)
        {
             await _httpClient.DeleteAsync($"api/Recipe/{recipeName}");
        }

    }
}