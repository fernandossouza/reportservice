using System.Collections.Generic;

namespace reportservice.Model{
    public class TabelaFront {        
        public int thingId {get; set;}
        public string groupTag { get; set; }
        public List<Dictionary<string, string>> data {get; set;}

        public TabelaFront(int thingId, string groupTag){
            this.thingId = thingId;
            this.groupTag = groupTag;
        }
    }
}                            