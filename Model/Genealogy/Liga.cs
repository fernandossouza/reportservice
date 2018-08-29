using System.Collections.Generic;

namespace reportservice.Model.Genealogy{
    public class Liga{
        public long id {get;set;}
        public long orderId {get;set;}
        public long orderNumber {get;set;}
        public long startDate {get;set;}                        
        public long endDate {get;set;}      
        public string code {get; set;}
        public string quantity {get; set;}
        public string batch {get; set;}                  
        public List<Elemento> productsInput {get;set;}
    }
}