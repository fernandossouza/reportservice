

using System.Collections.Generic;

namespace reportservice.Model.Genealogy{
    public class Rolo{

        public string id {get;set;}
        public string productId {get;set;}
        public string product {get;set;}
        public string quantity {get;set;}
        public string batch {get;set;}
        public long date {get;set;}        
        public string username {get;set;}
        public string unity {get;set;}
        public List<Rolo> rolosEntrada {get;set;}
    
    }
}