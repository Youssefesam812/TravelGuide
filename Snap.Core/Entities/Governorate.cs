using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

public class Governorate
{
    [Key]
    public int Id { get; set; }

    [Required]
    [MaxLength(255)]
    public string Name { get; set; }

    public string ImageUrl { get; set; }

    public string Description { get; set; }

    // Navigation property: One Governorate has many TopPlaces
    public List<TopPlace> TopPlaces { get; set; } = new List<TopPlace>();
}
