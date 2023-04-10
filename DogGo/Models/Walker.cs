using System.ComponentModel;

namespace DogGo.Models;

public class Walker
{
    public int Id { get; set; }
    public string Name { get; set; }

    [DisplayName("Neighborhood Id")]
    public int NeighborhoodId { get; set; }

    [DisplayName("Image")]
    public string ImageUrl { get; set; }
    public Neighborhood Neighborhood { get; set; }

    public List<Dog> Dogs { get; set; } = new List<Dog>();
}
