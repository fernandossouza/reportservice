using reportservice.Service.Interface;
using reportservice.Model;
using Microsoft.Extensions.Configuration;
using System.Threading.Tasks;
using System.Net.Http;
using System.Net.Http.Headers;
using System;
using System.Web;
using System.Net;
using Newtonsoft.Json;
using System.Collections.Generic;
using reportservice.Model.ProductionOrder;
using reportservice.Model.Quality;

namespace reportservice.Service
{
    public class OtherAPIService : IOtherAPIService
    {
        private readonly IConfiguration _configuration;
        private HttpClient client;

        public OtherAPIService (IConfiguration configuration)
        {
            _configuration = configuration;
            client = new HttpClient();
        }

        public async Task<List<HistState>> GetHistState(int productionOrderId)
        {
            List<HistState> histStateList = null;
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            var builder = new UriBuilder(_configuration["productionOrdersHistStates"]);
            var query = HttpUtility.ParseQueryString(builder.Query);

            builder.Query = query.ToString();
            string url = builder.ToString() + "?productionOrderId=" + productionOrderId;
           
            var result = await client.GetAsync(url);

            if(result.StatusCode == HttpStatusCode.OK)
            {
                histStateList = JsonConvert.DeserializeObject<List<HistState>>(await client.GetStringAsync(url));
            }

            return histStateList;               
        }

        public async Task<ProductionOrder> GetProductionOrderPerId(int productionOrderId)
        {
            ProductionOrder productionOrder = null;
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            var builder = new UriBuilder(_configuration["productionOrder"]);
            var query = HttpUtility.ParseQueryString(builder.Query);

            builder.Query = query.ToString();
            string url = builder.ToString() + productionOrderId;
           
            var result = await client.GetAsync(url);

            if(result.StatusCode == HttpStatusCode.OK)
            {
                productionOrder = JsonConvert.DeserializeObject<ProductionOrder>(await client.GetStringAsync(url));
            }

            return productionOrder;  
        }

        public async Task<ProductionOrderQuality> GetQualityPerProductionOrderId(int productionOrderId)
        {
            ProductionOrderQuality productionOrderQuality = null;
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            var builder = new UriBuilder(_configuration["qualityService"]);
            var query = HttpUtility.ParseQueryString(builder.Query);

            builder.Query = query.ToString();
            string url = builder.ToString() + productionOrderId;
           
            var result = await client.GetAsync(url);

            if(result.StatusCode == HttpStatusCode.OK)
            {
                productionOrderQuality = JsonConvert.DeserializeObject<ProductionOrderQuality>(await client.GetStringAsync(url));
            }

            return productionOrderQuality;  
        }

        public async Task<List<ProductionOrderQuality>> GetQualityPerRecipeCode(string recipeCode, long startDate, long endDate)
        {
            List<ProductionOrderQuality> productionOrderQualityList = null;
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            var builder = new UriBuilder(_configuration["qualityServiceRecipeCode"]);
            var query = HttpUtility.ParseQueryString(builder.Query);

            builder.Query = query.ToString();
            string url = builder.ToString() + recipeCode + "?startDate="+ startDate.ToString() +"&endDate=" + endDate.ToString();
           
            var result = await client.GetAsync(url);

            if(result.StatusCode == HttpStatusCode.OK)
            {
                productionOrderQualityList = JsonConvert.DeserializeObject<List<ProductionOrderQuality>>(await client.GetStringAsync(url));
            }

            return productionOrderQualityList;  
        }

        public async Task<List<ProductionOrderQuality>> GetQualityPerDate(long startDate, long endDate)
        {
            List<ProductionOrderQuality> productionOrderQualityList = null;
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            var builder = new UriBuilder(_configuration["qualityServiceDate"]);
            var query = HttpUtility.ParseQueryString(builder.Query);

            builder.Query = query.ToString();
            string url = builder.ToString() +"?startDate="+ startDate.ToString() +"&endDate=" + endDate.ToString();
           
            var result = await client.GetAsync(url);

            if(result.StatusCode == HttpStatusCode.OK)
            {
                productionOrderQualityList = JsonConvert.DeserializeObject<List<ProductionOrderQuality>>(await client.GetStringAsync(url));
            }

            return productionOrderQualityList;  
        }

        public async Task<Report> GetReportAPI (int thingId, long startDate, long endDate)
        {
            Report report = null;
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            var builder = new UriBuilder(_configuration["historianBigTable"]);
            var query = HttpUtility.ParseQueryString(builder.Query);


            builder.Query = query.ToString();
            string url = builder.ToString() + "?thingId=" + thingId;
            url = url + "&startDate="+ startDate;
            url = url + "&endDate="+ endDate;
           
            var result = await client.GetAsync(url);

            if(result.StatusCode == HttpStatusCode.OK)
            {
                report = JsonConvert.DeserializeObject<Report>(await client.GetStringAsync(url));
            }

            return report;            
        }

        public async Task<List<int>> GetHistStateProductionOrderList(string status,long startDate,long endDate)
        {
            List<int> productionOrderIdList = null;
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            var builder = new UriBuilder(_configuration["productionOrderIdListHistStates"]);
            var query = HttpUtility.ParseQueryString(builder.Query);

            builder.Query = query.ToString();
            string url = builder.ToString() + "?statusSearch=" + status;
            url = url + "&startDate="+ startDate;
            url = url + "&endDate="+ endDate;
           
            var result = await client.GetAsync(url);

            if(result.StatusCode == HttpStatusCode.OK)
            {
                productionOrderIdList = JsonConvert.DeserializeObject<List<int>>(await client.GetStringAsync(url));
            }

            return productionOrderIdList;
        }

        
        
    }
}