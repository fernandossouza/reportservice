using System.Collections.Generic;

namespace reportservice.Model.Genealogy
{
    public class RoloOutput
    {
        public RoloOutput(long startDate, long endDate, string nRolo, List<OrderLiga> ordersLiga, List<Aco> rolosInput){
            this.startDate = startDate;
            this.endDate = endDate;
            this.ordersLiga = ordersLiga;
            this.rolosInput = rolosInput;
            this.nRolo = nRolo;
        }        
        public long startDate {get;set;}
        public long endDate {get;set;}
        public List<OrderLiga> ordersLiga {get;set;}
        public List<Aco> rolosInput {get;set;}
        public string nRolo {get;set;}
    }
}