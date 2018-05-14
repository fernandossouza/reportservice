using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;

namespace reportservice.Model.ProductionOrder
{
    public class Product
    {
        [Key]
        public int productId { get; set; }
        [Required]
        [MaxLength(50)]
        public string productName { get; set; }
        [MaxLength(100)]
        public string productDescription { get; set; }
        [MaxLength(50)]
        public string productCode { get; set; }
        [MaxLength(50)]
        public string productGTIN { get; set; }
    }
}