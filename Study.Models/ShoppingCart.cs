using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Study.Models
{
    public class ShoppingCart
    {
        public int Id { get; set; }
        public int ProductId { get; set; }
        [ForeignKey("ProductId")]
        [ValidateNever]
        public Product Product { get; set; }
        [Range(1, 10, ErrorMessage = "Değer aralığı 1-10 arası olmalıdır.")]
        public int Count { get; set; }
        public double Price { get; set; }
        public string ApplicationUserId { get; set; }

    }
}
