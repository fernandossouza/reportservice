namespace reportservice.Model{

    public class Alarm{
        public int historianId {get;set;}
        public int alarmId {get;set;}
        public int thingId {get;set;}
        public string alarmDescription{get;set;}
        public string alarmName{get;set;}
        public string alarmColor{get;set;}
        public long startDate{get;set;}
        public long endDate{get;set;}        
    }
}