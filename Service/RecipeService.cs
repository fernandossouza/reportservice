using System;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using reportservice.Model.ProductionOrder;
using reportservice.Service.Interface;

namespace reportservice.Service
{
    public class RecipeService : IRecipeService
    {
         private IConfiguration _configuration;
        private HttpClient client = new HttpClient();
        public RecipeService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task<(Recipe, HttpStatusCode)> getRecipe(int recipeId)
        {
            Recipe returnRecipe = null;
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            var builder = new UriBuilder(_configuration["RecipeServiceEndPoint"]  + recipeId);
            string url = builder.ToString();
            var result = await client.GetAsync(url);
            switch (result.StatusCode)
            {
                case HttpStatusCode.OK:
                    returnRecipe = JsonConvert.DeserializeObject<Recipe>(await client.GetStringAsync(url));
                    return (returnRecipe, HttpStatusCode.OK);
                case HttpStatusCode.NotFound:
                    return (returnRecipe, HttpStatusCode.NotFound);
                case HttpStatusCode.InternalServerError:
                    return (returnRecipe, HttpStatusCode.InternalServerError);
            }
            return (returnRecipe, HttpStatusCode.NotFound);

        }
        
    }
}