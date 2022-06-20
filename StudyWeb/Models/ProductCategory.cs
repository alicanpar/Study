using System.ComponentModel.DataAnnotations;

namespace StudyWeb.Models
{
    public class ProductCategory
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }

    }
}
