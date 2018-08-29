using System.ComponentModel.DataAnnotations.Schema;
namespace reportservice.Model.Genealogy
{
    public class Aco{
        public Aco(long outputRollId, long id, string quantity, string batch, long startDate, long endDate){            
            this.id = id;
            this.quantity = quantity;
            this.batch = batch;
            this.startDate = startDate;
            this.endDate = endDate;
        }

        
        public long id {get; set;}                
        public string code {get; set;}
        public string quantity {get; set;}
        public string batch {get; set;}
        public long startDate {get; set;}        
        public long endDate {get; set;}        
    }
}