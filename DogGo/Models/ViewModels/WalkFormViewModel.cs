using Microsoft.AspNetCore.Mvc.Rendering;

namespace DogGo.Models.ViewModels
{
    public class WalkFormViewModel
    {
        public DateTime Date { get; set; } = DateTime.Today;
        public int WalkerId { get; set; }
        public List<int> SelectedDogs { get; set; } = new List<int>();
        public int Duration { get; set; }
        public int Hours { get; set; }
        public int Minutes { get; set; }
        public List<SelectListItem> DogOptions { get; set; } = new List<SelectListItem>();
    }
}
