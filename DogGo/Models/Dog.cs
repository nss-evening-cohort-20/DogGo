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
        
        // ?? -> null coalesce operator, returns the item to the left if it isn't null, otherwise the item to the right
        public string DisplayImageUrl => ImageUrl ?? "https://external-content.duckduckgo.com/iu/?u=https%3A%2F%2Fstatic.vecteezy.com%2Fsystem%2Fresources%2Fpreviews%2F000%2F567%2F006%2Foriginal%2Fvector-dog-icon.jpg&f=1&nofb=1&ipt=00fe2348878b72432ddba17f9d887dbb893502a9293f4722d10682a07af4ca39&ipo=images";
    }
}
