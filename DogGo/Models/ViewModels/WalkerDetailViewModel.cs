using DogGo.Utilities;

namespace DogGo.Models.ViewModels
{
    public class WalkerDetailViewModel
    {
        public Walker Walker { get; set; }
        public string TotalWalkTime => ViewHelpers.DurationToText(Walker.Walks.Sum(w => w.Duration));
    }
}
