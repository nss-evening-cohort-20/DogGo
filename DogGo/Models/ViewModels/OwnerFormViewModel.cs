using Microsoft.AspNetCore.Mvc.Rendering;

namespace DogGo.Models.ViewModels
{
    public class OwnerFormViewModel
    {
        public Owner? Owner { get; set; }
        public List<SelectListItem> NeighborhoodOptions { get; set; } = new List<SelectListItem>();
    }
}
