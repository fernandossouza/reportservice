using System.Net;
using System.Threading.Tasks;
using reportservice.Model.ProductionOrder;

namespace reportservice.Service.Interface
{
    public interface IRecipeService
    {
         Task<(Recipe, HttpStatusCode)> getRecipe(int recipeId);
    }
}