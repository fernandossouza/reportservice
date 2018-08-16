
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
using reportservice.Model;

namespace reportservice.Service{

    public class GenealogyService : IGenealogyService {  
        
        private readonly IConfiguration configuration;
        private UriBuilder builder;
        private HttpClient client;

        public GenealogyService(IConfiguration configuration){            
            this.configuration = configuration;
            this.client = new HttpClient();
        }


        public async Task<(object, HttpStatusCode)> getGenealogyOpAsync(long startDate, long endDate, string op, string codigo, string fieldFilter){            
            string [] response = new string[1];
            if(fieldFilter == "op")
                response[0] = op;              
            else                                                
                response = await getIds(startDate, endDate);      
            Console.WriteLine("Buscando ordens");
            var(status, orders) = await getOrders(response);                
            if(fieldFilter == "cod")                   
                orders = orders.FindAll(x => x.recipe.recipeId.ToString() == codigo).ToList();       
            
            if(orders.Count<=0)
                return ("Nenhum registro encontrado !", HttpStatusCode.NotFound);         
            List<Genealogy> genealogys = new List<Genealogy>();            
            Console.WriteLine("Loop do mal");
            foreach(ProductionOrder order in orders){                                
                var (l1, l2) = await getProducts(order.productionOrderId);                                
                var(genealogy, status2) = await createGenealogy(l1, l2, order);                
                if(status2 == HttpStatusCode.OK)
                    genealogys.Add(genealogy);
            }
            return (genealogys, HttpStatusCode.OK);
        }


        public async Task<(Genealogy, HttpStatusCode)> createGenealogy(List<Rolo> productsInput, List<Rolo> productsOutput, ProductionOrder order){
            Genealogy genealogy = new Genealogy();            
            genealogy.orderId = order.productionOrderNumber;            
            genealogy.startDate = await getStartDate(order.productionOrderId);                                    
            long ini = genealogy.startDate;            
            genealogy.outputRolos = new List<RoloOutput>();            
            RoloOutput rolo;            
            if(productsInput == null || productsOutput == null )
                return (genealogy, HttpStatusCode.OK);

            foreach(Rolo r in productsOutput)  {   
                rolo = new RoloOutput(ini, r.date, r.batch,new List<OrderLiga>(), new List<Aco>());                
                foreach(Rolo r1 in productsInput.FindAll(c => c.date > rolo.startDate && c.date < r.date)){                     
                    builder = new UriBuilder(this.configuration["apiHistorian"]+r1.code);                                   
                    if(r1.productType == "liga" && (await client.GetAsync(builder.ToString())).StatusCode == HttpStatusCode.OK)                                                         
                        rolo.ordersLiga.Add(JsonConvert.DeserializeObject<OrderLiga>((await client.GetStringAsync(builder.ToString()))));                        
                    else
                        rolo.rolosInput.Add(new Aco(r1.batch, r1.quantity, r1.batch, r1.date)); 
                    rolo.endDate = r1.date;
                    ini = r1.date;                                                                  
                }   
                genealogy.outputRolos.Add(rolo);                                  
            }    
            genealogy.tools = await getTools(genealogy.startDate);                
            return (genealogy, HttpStatusCode.OK);
        }

       
        

        public async Task<List<Tool>> getTools(long startDate){            
            builder = new UriBuilder(this.configuration["historianBigTable"]+"?thingId=1&startDate="+startDate+"&endDate="+startDate+600000000); //startDate+600000000));
            Console.WriteLine(builder.ToString());
            if((await client.GetAsync(builder.ToString())).StatusCode != HttpStatusCode.OK)
                return null;            
            Console.WriteLine("Passou pela big ");                
            List<Tag> tags = JsonConvert.DeserializeObject<List<Tag>>(JObject.Parse(await client.GetStringAsync(builder.ToString()))["tags"].ToString());                         
            List<Tag> toolIds = tags.Where(c => c.name == "toolId").ToList();                               
            List<Tag> vida_utils = tags.Where(c => c.name == "vida_util").ToList();  /*Limpando memoria*/ tags = null;                        
            string[] vetTags = new string[toolIds.Count];int i=0;                        
            foreach(Tag t in toolIds)
                vetTags[i++] = t.value.First();                 
            List<Tool> tools = null;                                    
            builder = new UriBuilder(this.configuration["tool"]+"ids?ids="+string.Join(",", vetTags));
            Console.WriteLine(builder.ToString());
            if((await client.GetAsync(builder.ToString())).StatusCode == HttpStatusCode.OK)
                tools = JsonConvert.DeserializeObject<List<Tool>>(await client.GetStringAsync(builder.ToString()+string.Join(",", vetTags)));            
                             
            foreach(Tool t in tools) 
                t.group = toolIds.Where(c => c.value.Contains(t.toolId)).First().group;                                                                                
            foreach(Tool t in tools){                   
                t.vidaUtil = vida_utils.Where(x => x.group == t.group).FirstOrDefault().value.First();
            }    
            return tools;
        }

        public async Task<long> getStartDate(int id){
            builder = new UriBuilder(this.configuration["productionOrdersHistStates"]+"?ProductionOrderId="+id);      
            if((await client.GetAsync(builder.ToString())).StatusCode != HttpStatusCode.OK)
                return 0;
            var states = JsonConvert.DeserializeObject<List<HistState>>(await client.GetStringAsync(builder.ToString()));             
            HistState state = states.Where(x => x.state == "active").FirstOrDefault();            
            return (state.date);
        }


        public async Task<string []> getIds(long startDate, long endDate){
            builder = new UriBuilder(this.configuration["productionOrderIdListHistStates"]);    
            var query = "?StatusSearch=active&StartDate="+startDate+"&EndDate="+endDate;            
            return JsonConvert.DeserializeObject<string []>(await client.GetStringAsync(builder.ToString()+query));
        }


        public async Task<(List<Rolo>, List<Rolo>)> getProducts(long id){    
            Console.WriteLine(id);        
            builder = new UriBuilder(this.configuration["apiHistorian"]);             
            if((await client.GetAsync(builder.ToString()+id)).StatusCode == HttpStatusCode.NotFound)              
                return (null, null);            
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
            List<ProductionOrder> orders = JsonConvert.DeserializeObject<List<ProductionOrder>>(await client.GetStringAsync(builder.ToString()+query));
            if(orders == null || orders.Count<=0)
                return (HttpStatusCode.NoContent, orders);
            return (HttpStatusCode.OK, orders);
        }


    }
}        