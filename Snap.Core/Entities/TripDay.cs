using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Snap.Core.Entities
{
    public class TripDay
    {
        public int Id { get; set; }
        public string Day { get; set; } = null!;
        public string ApproximateCost { get; set; } = null!;

        public string? Notes { get; set; }

        // Foreign Key
        public int TripPlanId { get; set; }
        public TripPlan TripPlan { get; set; } = null!;

        public ICollection<TripActivity> Activities { get; set; } = new List<TripActivity>();
    }
}
