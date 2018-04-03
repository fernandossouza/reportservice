using System.Collections.Generic;

namespace reportservice.Model
{
    public class Report
    {
        public int thingId{get;set;}
        public string thingName{get;set;}
        public List<Tag> tags{get;set;}

    }
}