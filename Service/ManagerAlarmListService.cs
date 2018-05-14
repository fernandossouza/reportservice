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
        public ManagerAlarmListService (IConfiguration configuration){
            this.configuration = configuration;
            client = new HttpClient();
        }


        public async Task<(List<AlarmFront>, HttpStatusCode)> getAlarms(int thingId, long startDate, long endDate){            
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));            
            string query="?thingId="+thingId+"&startDate="+startDate+"&endDate="+endDate+"&startat=0&quantity=500";            
            var builder = new UriBuilder(this.configuration["historianAlarm"]);                                                 
            string url = builder.ToString();   
            Console.WriteLine("Conectando api historianAlarm");
            List<Alarm> listaAlarm  = JsonConvert.DeserializeObject<List<Alarm>>(JObject.Parse(await client.GetStringAsync(url+query))["values"].ToString());            
            Console.WriteLine("Conectou retorno = "+ JsonConvert.SerializeObject(listaAlarm));
            Console.WriteLine("Ordenando lista");
            listaAlarm = listaAlarm.OrderBy(c => c.alarmDescription).ToList(); AlarmFront alarm;            
            Console.WriteLine("Ordenou retorno = "+ JsonConvert.SerializeObject(listaAlarm));
            List<AlarmFront> lista = new List<AlarmFront>(); 
            var nomes = new HashSet<string>();                                    
            foreach(Alarm a in listaAlarm)
                nomes.Add(a.alarmName);     
            int i=0;
            try{                                                                
                foreach (string item in nomes){ 
                                                                            
                    var g = (from t in listaAlarm where t.alarmName == item select t).ToList();                                
                    var group = g.GroupBy(x=>new DateTime(x.startDate).Date);   
                    alarm = new AlarmFront(); alarm.data = new List<Dictionary<string, string>>();    
                    alarm.groupTag = item;  
                    alarm.thing = thingId.ToString();
                    Console.WriteLine("Inicio do Loop de grupos por data");
                    foreach(var s in group){ 
                        //Console.WriteLine("teste");   
                        //Console.WriteLine(s.Where(x => x.alarmName == "Alto").Count().ToString());                                    
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
                    Console.WriteLine("Adicionou "+ i + "elemento na lista");
                    lista.Add(alarm);   
                }   
            }catch(Exception e){
                Console.WriteLine();Console.WriteLine();
                Console.Write("Erro : ");
                Console.Write(e);
                Console.WriteLine();Console.WriteLine();
                return(null, HttpStatusCode.BadRequest);
            }                        
            return (lista, HttpStatusCode.OK);   
        }
    }
}                   