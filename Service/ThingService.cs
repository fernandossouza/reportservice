using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using reportservice.Model.Thing;
using reportservice.Service.Interface;

namespace reportservice.Service
{
    public class ThingService : IThingService
    {
        private IConfiguration _configuration;
        private HttpClient client = new HttpClient();
        public ThingService(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        public async Task<(List<Thing>, HttpStatusCode)> getThing(int? startat, int? quantity,
            string fieldFilter=null, string fieldValue = null,
            string orderField=null, string order=null)
        {
            string query="?";
            List<Thing> returnThing = null;
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            var builder = new UriBuilder(_configuration["thingServiceEndpoint"]);
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
                    returnThing = JsonConvert.DeserializeObject<List<Thing>>(returnJson);
                    return (returnThing, HttpStatusCode.OK);
                case HttpStatusCode.NotFound:
                    return (returnThing, HttpStatusCode.NotFound);
                case HttpStatusCode.InternalServerError:
                    return (returnThing, HttpStatusCode.InternalServerError);
            }
            return (returnThing, HttpStatusCode.NotFound);

        }
        
    }
}