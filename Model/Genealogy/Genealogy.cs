
using System.Collections.Generic;

namespace reportservice.Model.Genealogy{
    public class Genealogy{        
        public long orderId { get ; set ; }        
        public string productionOrderNumber { get ; set ; }    
        public string productionOrderId { get; set; }    
        public long startDate { get; set; }        
        public long endDate { get; set; }        
        public List<EndRoll> outputRolls {get;set;}            
    }
}