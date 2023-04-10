using System.ComponentModel.DataAnnotations;

namespace DogGo.Models;

public class Owner
{
    public int Id { get; set; }
    
    [StringLength(255)]
    public string Email { get; set; }

    [StringLength(55)]
    public string Name { get; set; }

    [StringLength(255)]
    public string Address { get; set; }

    public int? NeighborhoodId { get; set; }

    [StringLength(55)]
    public string Phone { get; set; }
    public List<Dog> Dogs { get; set; } = new List<Dog>();
}
