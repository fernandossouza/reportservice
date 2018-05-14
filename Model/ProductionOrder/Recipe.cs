using System.Collections.Generic;

namespace reportservice.Model.ProductionOrder
{
    public class Recipe
    {
        public int recipeId{get;set;}
        public string recipeCode{get;set;}
        public ICollection<Phase> phases { get; set; }
    }
}