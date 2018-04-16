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
    public class ProductionOrderService : IProductionOrderService
    {
        private IConfiguration _configuration;
        private HttpClient client = new HttpClient();
        public ProductionOrderService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task<(List<ProductionOrder>, HttpStatusCode)> getProductionOrder(int? startat, int? quantity,
            string fieldFilter=null, string fieldValue=null,
            string orderField=null, string order=null)
        {
            string query = "?";
            List<ProductionOrder> returnProductionOrder = null;
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            var builder = new UriBuilder(_configuration["productionOrder"]);
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
                {
                    var returnAPI = await client.GetStringAsync(url);
                    returnProductionOrder = JsonConvert.DeserializeObject<List<ProductionOrder>>(JObject.Parse(returnAPI)["values"].ToString());
                    return (returnProductionOrder, HttpStatusCode.OK);
                }
                case HttpStatusCode.NotFound:
                    return (returnProductionOrder, HttpStatusCode.NotFound);
                case HttpStatusCode.InternalServerError:
                    return (returnProductionOrder, HttpStatusCode.InternalServerError);
            }
            return (returnProductionOrder, HttpStatusCode.NotFound);

        }


    }
}