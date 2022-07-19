using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace Study.Models
{ 
    public class ApplicationUser
    {
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        public string? StreetAddress { get; set; }
        public string? City { get; set; }
        public string? State { get; set; }
        public string? PostaCode { get; set; }
        public string? PhoneNumber { get; set; }
        public string? UserName { get; set; }
    }
}
