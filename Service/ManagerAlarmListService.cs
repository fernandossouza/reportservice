using System;
using System.Collections.Generic;
using System.Linq;
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
    public class ManagerAlarmListService : IManagerAlarmListService{
        private readonly IConfiguration configuration;
        private HttpClient client;
        public int thingId { get; set; }
        public long startDate { get; set; }
        public long endDate { get; set; }
        public ManagerAlarmListService (IConfiguration configuration){
            this.configuration = configuration;
            client = new HttpClient();
        }


        public async Task<(List<AlarmFront>, HttpStatusCode)> getAlarms(){            
            try{
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));            
                string query="?thingId="+this.thingId+"&startDate="+this.startDate+"&endDate="+this.endDate+"&startat=0&quantity=500";
                UriBuilder builder = new UriBuilder(this.configuration["historianAlarm"]);             
                Console.WriteLine("Conectando api historianAlarm"+this.configuration["historianAlarm"]);
                var json = JObject.Parse(await client.GetStringAsync(builder.ToString()+query))["values"].ToString();
                List<Alarm> listaAlarm  = JsonConvert.DeserializeObject<List<Alarm>>(json);            
                Console.WriteLine("Conectou retorno = "+ JsonConvert.SerializeObject(listaAlarm));
                Console.WriteLine("Ordenando lista");
                listaAlarm = listaAlarm.OrderBy(c => c.alarmDescription).ToList(); AlarmFront alarm;            
                Console.WriteLine("Ordenou retorno = "+ JsonConvert.SerializeObject(listaAlarm));
                List<AlarmFront> lista = new List<AlarmFront>(); 
                var nomes = new HashSet<string>();                                    
                foreach(Alarm a in listaAlarm)
                    nomes.Add(a.alarmName);     
                int i=0;                                                                            
                foreach (string item in nomes){                                                                            
                    var g = (from t in listaAlarm where t.alarmName == item select t).ToList();                                
                    var group = g.GroupBy(x=>new DateTime(x.startDate).Date);   
                    alarm = new AlarmFront(); alarm.data = new List<Dictionary<string, string>>();    
                    alarm.groupTag = item;  
                    alarm.thing = thingId.ToString();
                    Console.WriteLine("Inicio do Loop de grupos por data");
                    foreach(var s in group){                                   
                        Console.WriteLine("Contando alarmes");
                        alarm.data.Add(new Dictionary<string, string>{
                            ["data"] =  s.First().startDate.ToString(),
                            ["muito alto"] = s.Where(x => x.alarmDescription.ToLower() == "muito alto").Count().ToString(),
                            ["alto"] = s.Where(x => x.alarmDescription.ToLower() == "alto").Count().ToString(),
                            ["baixo"] = s.Where(x => x.alarmDescription.ToLower() == "baixa").Count().ToString(),
                            ["muito baixo"] = s.Where(x => x.alarmDescription.ToLower() == "muito alto").Count().ToString(),
                            ["offline"]  = s.Where(x => x.alarmDescription.ToLower() == "offline").Count().ToString()                     
                        });
                    }                 
                    i++;
                    Console.WriteLine("Adicionou "+ i + "elemento na lista de dictionary");
                    lista.Add(alarm);                       
                }   
                return (lista, HttpStatusCode.OK); 
            }catch(Exception e){
                Console.WriteLine();Console.WriteLine();
                Console.Write("Erro : ");
                Console.Write(e);
                Console.WriteLine();Console.WriteLine();
                return(null, HttpStatusCode.BadRequest);
            }                                      
        }

        public async Task<(List<AlarmFront>, HttpStatusCode)> defineGet(int opId, int thingId, long startDate, long endDate){
            this.thingId = thingId;
            if(opId == 0){   
                this.startDate = startDate;
                this.endDate = endDate;
                return await getAlarms();
            }else{
                var builder = new UriBuilder(this.configuration["productionOrdersHistStates"]);
                string url = builder.ToString();
                var result = await client.GetStringAsync(url+"?ProductionOrderId="+opId);
                List<HistState> listaOrders  = JsonConvert.DeserializeObject<List<HistState>>(result);            
                int i = listaOrders.FindIndex(x => x.state == "active");
                int j = listaOrders.FindIndex(x => x.state == "ended");
                this.startDate = listaOrders.ElementAt(i).date;
                if(i>=0)                    
                    this.endDate = listaOrders.ElementAt(j).date;
                else
                    this.endDate = DateTime.Now.Ticks;
                return await getAlarms();   
            }
        }        
    }
}                   