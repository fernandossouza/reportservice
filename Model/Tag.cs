using System.Collections.Generic;

namespace reportservice.Model{
    public class Tag{
        public string name{get;set;}
        public string color{get;set;}
        public string group{get;set;}
        public List<long> timestamp {get;set;}
        public List<string> value{get;set;}
    }
}