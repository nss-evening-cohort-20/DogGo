using System.ComponentModel.DataAnnotations;

namespace DogGo.Models
{
    public class Dog
    {

        public int Id { get; set; }
        
        [StringLength(55)]
        public string Name { get; set; } = string.Empty;

        public int OwnerId { get; set; }

        [StringLength(55)]
        public string Breed { get; set; } = string.Empty;
        
        [StringLength(255)]
        public string? Notes { get; set; }
        
        [StringLength(255)]
        public string? ImageUrl { get; set; }
    }
}
