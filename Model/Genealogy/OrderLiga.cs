using System.Collections.Generic;

namespace reportservice.Model.Genealogy
{
    public class OrderLiga
    {
        public string id {get;set;}
        public string order {get;set;}
        public List<Elemento> productsInput {get;set;}
    }
}