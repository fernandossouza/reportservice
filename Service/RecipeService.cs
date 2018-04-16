using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
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

        public async Task<(List<Recipe>, HttpStatusCode)> getRecipe(int? startat, int? quantity,
            string fieldFilter=null, string fieldValue=null,
            string orderField=null, string order=null)
        {
            string query="?";
            List<Recipe> returnRecipe = null;
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            var builder = new UriBuilder(_configuration["RecipeServiceEndPoint"]);
            string url = builder.ToString();
            if(startat != null)
                query = query + "&startat="+startat.ToString();
            if(quantity != null)
                query = query + "&quantity="+quantity.ToString();
            if(fieldFilter != null)
                query = query + "&fieldFilter="+fieldFilter.ToString();
            if(fieldValue != null)
                query = query + "&fieldValue="+fieldValue.ToString();
            if(orderField != null)
                query = query + "&orderField="+orderField.ToString();
            if(order != null)
                query = query + "&order="+order.ToString();

            if(query.Length>1)
                url = url+query;
            var result = await client.GetAsync(url);
            switch (result.StatusCode)
            {
                case HttpStatusCode.OK:
                    var returnJson = await client.GetStringAsync(url);
                    returnRecipe = JsonConvert.DeserializeObject<List<Recipe>>(JObject.Parse(returnJson)["values"].ToString());
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