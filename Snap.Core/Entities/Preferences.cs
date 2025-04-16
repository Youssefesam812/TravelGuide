using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Snap.Core.Entities
{
    public class Preferences
    {
        [Key]
        [ForeignKey("User")]
        public string UserId { get; set; }  // Matches User.Id

        public string? PreferredPlaces { get; set; }  // JSON string or comma-separated
        public string? TravelTags { get; set; }       // JSON string or comma-separated
        public int? Age { get; set; }
        public string? MaritalStatus { get; set; }
        public string? Children { get; set; }         // "Yes", "No", or null
        public string? Gender { get; set; }

        // Navigation property
        public virtual User User { get; set; }
    }
}
