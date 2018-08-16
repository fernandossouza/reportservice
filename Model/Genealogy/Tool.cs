using System.Collections.Generic;

namespace reportservice.Model.Genealogy{
    public class Tool{
        public Tool(string toolId, string typeName, string serialNumber, string vidaUtil){
            this.toolId = toolId;
            this.typeName = typeName;
            this.serialNumber = serialNumber;
            this.vidaUtil = vidaUtil;
        }
        public string toolId {get;set;}
        public string typeName{get;set;}
        public string serialNumber{get;set;}
        public string vidaUtil{get;set;}
        public string group{get; set;}
    }
}    