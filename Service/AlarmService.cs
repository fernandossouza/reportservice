
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using reportservice.Model;
using reportservice.Service.Interface;

namespace reportservice.Service{

    public class AlarmService : IAlarmService{        
        private IConfiguration _configuration; 
        private HttpClient client = new HttpClient();        
        public AlarmService(IConfiguration configuration){
            _configuration = configuration;
        }

        public async Task<(List<Alarm>, HttpStatusCode)> getAlarm(int thingId, long startDate, long endDate){             
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));            
            string query="?thingId="+thingId+"&startDate="+startDate+"&endDate="+endDate+"&startat=0&quantity=500";            
            var builder = new UriBuilder(_configuration["historianAlarm"]);             
            string url = builder.ToString();            
            if(query.Length>1)
                url = url+query;       
            Console.WriteLine(url);      
            var result = await client.GetStringAsync(url);//.GetAsync(url);            
            Console.WriteLine(result);            
            return (JsonConvert.DeserializeObject<List<Alarm>>(JObject.Parse(result)["values"].ToString()),HttpStatusCode.OK);
        }
    }
}