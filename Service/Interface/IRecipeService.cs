using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using reportservice.Model.ProductionOrder;

namespace reportservice.Service.Interface
{
    public interface IRecipeService
    {
         Task<(List<Recipe>, HttpStatusCode)> getRecipe(int? startat, int? quantity,
            string fieldFilter=null, string fieldValue=null,
            string orderField=null, string order=null);
    }
}