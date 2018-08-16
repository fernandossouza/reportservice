
using System.Collections.Generic;

namespace reportservice.Model.Genealogy{
    public class Genealogy{

        public string orderId {get;set;}
        public long startDate {get;set;}
        public long endDate {get;set;}
        public List<RoloOutput> outputRolos {get;set;}
        public List<Tool> tools {get;set;}
       
    }
}