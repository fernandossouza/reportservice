using System.Collections.Generic;
using reportservice.Model.Genealogy;

namespace reportservice.Model.ProductionOrder
{
    public class ProductionOrder
    {
        public int productionOrderId{get;set;}
        public string productionOrderNumber{get;set;}
        public Recipe recipe{get;set;}        
        public List<Rolo> rolosSaida{get;set;}

    }
}