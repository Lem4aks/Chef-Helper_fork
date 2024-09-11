using Chef_Helper_API;
using Chef_Helper_API.Models;

namespace Chef_Helper_Web.Services
{
    public interface IRecipesService
    {
        Task<List<Recipes>> GetRecipes();
        Task<Recipes> GetRecipes(string name);
        Task <Recipes> Update(string recipeToUpdate, Recipes recipe);

        Task<Recipes> Post(Recipes recipe);
        Task<List<RecipeWithQuantity>> GetRecipesAvailable();

        Task Delete(string name);
    }
}
