using DogGo.Utilities;
using System.ComponentModel;

namespace DogGo.Models;

public class Walker
{
    public int Id { get; set; }
    public string Name { get; set; }

    [DisplayName("Neighborhood Id")]
    public int NeighborhoodId { get; set; }

    public string RegistrationId { get; set; }

    [DisplayName("Image")]
    public string ImageUrl { get; set; }
    public Neighborhood Neighborhood { get; set; }

    public List<Dog> Dogs { get; set; } = new List<Dog>();
    public List<Walk> Walks { get; set; } = new List<Walk>();
    public string TotalWalkTime => ViewHelpers.DurationToText(Walks.Sum(w => w.Duration));
}
