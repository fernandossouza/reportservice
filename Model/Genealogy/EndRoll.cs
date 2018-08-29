using System.Collections.Generic;

namespace reportservice.Model.Genealogy{
    public class EndRoll{
        public string id {get;set;}
        public string productionOrderId {get;set;}    
        public List<Aco> inputRolls {get;set;}
        public List<Liga> ligas {get;set;}
        public List<Tool> tools {get;set;}
        public long startDate {get;set;}
        public long endDate {get;set;}            
    }
}