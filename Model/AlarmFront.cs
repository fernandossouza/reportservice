using System.Collections.Generic;

namespace reportservice.Model{
    public class AlarmFront {
        public AlarmFront(){

        }
        public string thing {get; set;}
        public string groupTag {get; set;}
        public List<Dictionary<string, string>> data {get; set;}         
    } 
}