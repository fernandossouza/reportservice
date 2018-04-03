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

        
        
    }
}