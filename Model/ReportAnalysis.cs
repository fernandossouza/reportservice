namespace reportservice.Model
{
    public class ReportAnalysis
    {
        public long date{get;set;}
        public string op{get;set;}
        public int numberAnalysis{get;set;}
        public int productId{get;set;}
        public string productName{get;set;}
        public double recipeMin{get;set;}
        public double recipeMax{get;set;}
        public double resultAnalysis{get;set;}
        public string correction{get;set;}
        public string status{get;set;}
        public string userName{get;set;}
    }
}