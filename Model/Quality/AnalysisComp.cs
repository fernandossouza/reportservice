using System.ComponentModel.DataAnnotations;
namespace reportservice.Model.Quality
{
    public class AnalysisComp
    {
        public int analysisCompId{get;set;}
        [Required]
        public int productId{get;set;}
        public string productName{get;set;}
        [Required]
        public double value{get;set;}
        public double valueKg{get;set;}
    }
}