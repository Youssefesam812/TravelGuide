using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Snap.Core.Entities
{
    public class TripActivity
    {
        public int Id { get; set; }
        public string Activity { get; set; } = null!;
        public string Location { get; set; } = null!;
        public string PriceRange { get; set; } = null!;
        public string Time { get; set; } = null!;

        // Foreign Key
        public int TripDayId { get; set; }
        public TripDay TripDay { get; set; } = null!;
    }
}
