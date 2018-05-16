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
        private int thingId;
        private long startDate;
        private long endDate;
        private List<AlarmFront> alarms = new List<AlarmFront>(); 
        private List<TabelaFront> tabelas = new List<TabelaFront>();
        private List<Alarm> listaAlarm;
        private AlarmFront alarm;   
        private TabelaFront tabela;
        private HashSet<string> nomes = new HashSet<string>(); 
        private RelatorioAlarm relatorio;

        public ManagerAlarmListService (IConfiguration configuration){
            this.configuration = configuration;
            client = new HttpClient();
        }

        public async Task<(RelatorioAlarm, HttpStatusCode)> getAlarms(){            
            try{
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));            
                string query="?thingId="+this.thingId+"&startDate="+this.startDate+"&endDate="+this.endDate+"&startat=0&quantity=500";
                UriBuilder builder = new UriBuilder(this.configuration["historianAlarm"]);             
                Console.WriteLine("Conectando api historianAlarm"+this.configuration["historianAlarm"]);
                var json = JObject.Parse(await client.GetStringAsync(builder.ToString()+query))["values"].ToString();
                this.listaAlarm  = JsonConvert.DeserializeObject<List<Alarm>>(json);            
                Console.WriteLine("Conectou o retorno foi = "+ JsonConvert.SerializeObject(listaAlarm));
                Console.WriteLine("Ordenando lista");                     
                Console.WriteLine("Ordenou retorno = "+ JsonConvert.SerializeObject(this.listaAlarm));                
                int i=0;   

                foreach(Alarm a in this.listaAlarm)
                    nomes.Add(a.alarmName);     
                                                                                            
                foreach (string item in nomes){                                                                            
                    var g = (from t in listaAlarm where t.alarmName == item select t).ToList();
                    g = g.OrderBy(c => c.alarmDescription).ToList();
                    tabela = new TabelaFront(thingId, item);
                    tabela.data = new List<Dictionary<string, string>>();
                    foreach (var s in g){
                        tabela.data.Add(new Dictionary<string, string>{
                            ["dateIni"] =  s.startDate.ToString(),
                            ["dateEnd"] = s.endDate.ToString(),
                            ["type"] = s.alarmDescription                                               
                        });
                    }
                    tabelas.Add(tabela);
                    var group = g.GroupBy(x=>new DateTime(x.startDate).Date); 
                    alarm = new AlarmFront(thingId.ToString(), item);                       
                    alarm.data = new List<Dictionary<string, string>>();                       
                    Console.WriteLine("Inicio do Loop de grupos por data");
                    foreach(var s in group){                                   
                        Console.WriteLine("Contando alarmes");
                        alarm.data.Add(new Dictionary<string, string>{
                            ["category"] =  s.First().startDate.ToString(),
                            ["muito alto"] = s.Where(x => x.alarmDescription.ToLower() == "muito alto").Count().ToString(),
                            ["alto"] = s.Where(x => x.alarmDescription.ToLower() == "alto").Count().ToString(),
                            ["baixo"] = s.Where(x => x.alarmDescription.ToLower() == "baixa").Count().ToString(),
                            ["muito baixo"] = s.Where(x => x.alarmDescription.ToLower() == "muito alto").Count().ToString(),
                            ["offline"]  = s.Where(x => x.alarmDescription.ToLower() == "offline").Count().ToString()                     
                        });
                    }                 
                    i++;
                    Console.WriteLine("Adicionou "+ i + "elemento na lista de dictionary");
                    alarms.Add(alarm);                       
                }   
                relatorio = new RelatorioAlarm(alarms, tabelas);
                return (relatorio, HttpStatusCode.OK); 
            }catch(Exception e){
                Console.WriteLine();Console.WriteLine();
                Console.Write("Erro : ");
                Console.Write(e);
                Console.WriteLine();Console.WriteLine();
                return(null, HttpStatusCode.BadRequest);
            }                                      
        }

        public async Task<(RelatorioAlarm, HttpStatusCode)> defineGet(int opId, int thingId, long startDate, long endDate){
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