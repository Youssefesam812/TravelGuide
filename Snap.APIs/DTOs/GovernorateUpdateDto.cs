namespace Snap.APIs.DTOs
{
    public class GovernorateUpdateDto
    {
        public string? Name { get; set; }
        public string? ImageUrl { get; set; }
        public string? Description { get; set; }

        public List<TopPlaceUpdateDto>? TopPlaces { get; set; }
    }
}
