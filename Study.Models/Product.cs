using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Study.Models
{
    public class Product
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public int SerialNumber { get; set; }
        [Required]
        public int ImeiCode { get; set; }
        [Required]
        public string Color { get; set; }
        [Required]
        public string Mark { get; set; }
        [Required]
        public double Size { get; set; }
        [ValidateNever]
        public string ImageUrl { get; set; }
        public double Price { get; set; }
        public int ProductCategoryId { get; set; }
        [ForeignKey("ProductCategoryId")]
        [ValidateNever]
        public ProductCategory ProductCategory { get; set; }

    }
}
