using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

public class TopPlace
{
    [Key]
    public int Id { get; set; }

    [Required]
    [MaxLength(255)]
    public string Name { get; set; }

    public string ImageUrl { get; set; }

    // Foreign key to Governorate
    [ForeignKey("Governorate")]
    public int GovernorateId { get; set; }

    [JsonIgnore]
    public Governorate Governorate { get; set; }
}
