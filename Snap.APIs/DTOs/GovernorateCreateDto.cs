namespace Snap.APIs.DTOs
{
    public class GovernorateCreateDto
    {
        public string Name { get; set; }
        public string ImageUrl { get; set; }
        public string Description { get; set; }
        public List<TopPlaceCreateDto> TopPlaces { get; set; } = new();
    }
}
