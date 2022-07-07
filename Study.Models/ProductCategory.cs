using System.ComponentModel.DataAnnotations;

namespace Study.Models
{
    public class ProductCategory
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string Type { get; set; }

    }
}
