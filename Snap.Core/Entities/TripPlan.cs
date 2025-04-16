using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.Json.Serialization;

namespace Snap.Core.Entities
{
    public class TripPlan
    {
        [Key]
        public int Id { get; set; }

        public string City { get; set; } = null!;
        public string Image { get; set; } = null!;
        public string VisitorType { get; set; } = null!;
        public string NumDays { get; set; } = null!;
        public string Budget { get; set; } = null!;

        // Foreign key to User
        [Required]
        public string UserId { get; set; } = null!;

        public ICollection<TripDay> TripDays { get; set; } = new List<TripDay>();

        [ForeignKey("UserId")]
        [JsonIgnore]
        public User User { get; set; } = null!;
    }
}
