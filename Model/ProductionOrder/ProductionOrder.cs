using System.Collections.Generic;

namespace reportservice.Model.ProductionOrder
{
    public class ProductionOrder
    {
        public int productionOrderId{get;set;}
        public string productionOrderNumber{get;set;}
        public Recipe recipe{get;set;}                
    }
}