namespace Snap.APIs.DTOs
{
    public class TripPlanDto
    {
        public int Id { get; set; }
        public string City { get; set; } = null!;
        public string Image { get; set; } = null!;
        public string VisitorType { get; set; } = null!;
        public string NumDays { get; set; } = null!;
        public string Budget { get; set; } = null!;
        public string UserId { get; set; } = null!;
    }

    public class TripDayDto
    {
        public string Day { get; set; } = null!;
        public string ApproximateCost { get; set; } = null!;
        public int TripPlanId { get; set; }
        public List<TripActivityDto> Activities { get; set; } = new List<TripActivityDto>();

        public string? Notes { get; set; }
    }

    public class TripActivityDto
    {
        public string Activity { get; set; } = null!;
        public string Location { get; set; } = null!;
        public string PriceRange { get; set; } = null!;
        public string Time { get; set; } = null!;
    }
}
