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
    public class ProductionOrderService : IProductionOrderService
    {
        private IConfiguration _configuration;
        private HttpClient client = new HttpClient();
        public ProductionOrderService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task<(ProductionOrder, HttpStatusCode)> getProductionOrder(int productionOrderId)
        {
            ProductionOrder returnProductionOrder = null;
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            var builder = new UriBuilder(_configuration["productionOrder"]  + productionOrderId);
            string url = builder.ToString();
            var result = await client.GetAsync(url);
            switch (result.StatusCode)
            {
                case HttpStatusCode.OK:
                    returnProductionOrder = JsonConvert.DeserializeObject<ProductionOrder>(await client.GetStringAsync(url));
                    return (returnProductionOrder, HttpStatusCode.OK);
                case HttpStatusCode.NotFound:
                    return (returnProductionOrder, HttpStatusCode.NotFound);
                case HttpStatusCode.InternalServerError:
                    return (returnProductionOrder, HttpStatusCode.InternalServerError);
            }
            return (returnProductionOrder, HttpStatusCode.NotFound);

        }


    }
}