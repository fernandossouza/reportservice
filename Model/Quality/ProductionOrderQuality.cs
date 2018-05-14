using System.Collections.Generic;

namespace reportservice.Model.Quality
{
    public class ProductionOrderQuality
    {
        public int productionOrderQualityId{get;set;}
        public string forno{get;set;}
        public int corrida{get;set;}
        public string posicao{get;set;}
        public int productionOrderId{get;set;}
        public string productionOrderNumber{get;set;}
        public string status{get;set;}
        public double qntForno{get;set;}
        public string CobreFosforosoAtual{get;set;}
        public List<MessageCalculates> calculateInitial{get;set;}
        public List<Analysis> Analysis{get;set;}
    }
}