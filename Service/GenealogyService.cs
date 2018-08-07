
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using reportservice.Model.ProductionOrder;
using reportservice.Model.Genealogy;
using reportservice.Service.Interface;

namespace reportservice.Service{

    public class GenealogyService : IGenealogyService {  
        
        private readonly IConfiguration configuration;
        private UriBuilder builder;
        private HttpClient client;

        public GenealogyService(IConfiguration configuration){            
            this.configuration = configuration;
            this.client = new HttpClient();
        }
        
        public async Task<(object, HttpStatusCode)> getGenealogyOpAsync(long startDate, long endDate){            
            string [] response = await getIds(startDate, endDate);
            if(response.Length<=0)
                return ("Nenhum registro encontrado !", HttpStatusCode.NotFound);            
            var(status, orders) = await getOrders(response);                
            //if(status == HttpStatusCode.OK)   
              //  return (orders,HttpStatusCode.OK);
            var (l1, l2) = await getProducts("48");  
            (object ob, object bo) = await createGenealogy(l1, l2);
            return (null, HttpStatusCode.OK);
        }

        public async Task<(string, HttpStatusCode)> createGenealogy(List<Rolo> productsInput, List<Rolo> productsOutput){
            long ini = 0;
            List<Rolo> materiaPrima = new List<Rolo>();            
            foreach(Rolo r in productsOutput)
                foreach(Rolo r1 in productsInput.FindAll(c => c.date > ini && c.date < r.date)){                
                    Console.WriteLine(r1.batch);
                    ini = r.date;
                }    
            return (null, HttpStatusCode.Unused);
        }

        public async Task<string []> getIds(long startDate, long endDate){
            builder = new UriBuilder(this.configuration["productionOrderIdListHistStates"]);    
            var query = "?StatusSearch=active&StartDate="+startDate+"&EndDate="+endDate;            
            return JsonConvert.DeserializeObject<string []>(await client.GetStringAsync(builder.ToString()+query));
        }


        public async Task<(List<Rolo>, List<Rolo>)> getProducts(string id){
            builder = new UriBuilder(this.configuration["apiHistorian"]);
            var json = await client.GetStringAsync(builder.ToString()+id);
            List<Rolo> productsInput = JsonConvert.DeserializeObject<List<Rolo>>(JObject.Parse(json)["productsInput"].ToString());
            List<Rolo> productsOutput = JsonConvert.DeserializeObject<List<Rolo>>(JObject.Parse(json)["productsOutput"].ToString());    
            productsOutput.Sort((x,y) => x.date.CompareTo(y.date));                                
            productsInput.Sort((x,y) => x.date.CompareTo(y.date));
            return (productsInput, productsOutput);
        }

        public async Task<(HttpStatusCode, List<ProductionOrder>)> getOrders(string[] ids){            
            builder =  new UriBuilder(this.configuration["productionOrder"]);            
            var query = "ids?ids=";            
            foreach(string value in ids)
                query += value+',';
            query = query.Substring(0, query.Length-1);       
            Console.WriteLine("Entrou no endpoint");
            Console.WriteLine(builder.ToString()+query);             
            List<ProductionOrder> orders = JsonConvert.DeserializeObject<List<ProductionOrder>>(await client.GetStringAsync(builder.ToString()+query));
            if(orders == null || orders.Count<=0)
                return (HttpStatusCode.NoContent, orders);
            return (HttpStatusCode.OK, orders);
        }


    }
}        