using System;
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
        public async Task<(Thing, HttpStatusCode)> getThing(int thingId)
        {
            Thing returnThing = null;
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            var builder = new UriBuilder(_configuration["thingServiceEndpoint"] + thingId);
            string url = builder.ToString();
            var result = await client.GetAsync(url);
            switch (result.StatusCode)
            {
                case HttpStatusCode.OK:
                    returnThing = JsonConvert.DeserializeObject<Thing>(await client.GetStringAsync(url));
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