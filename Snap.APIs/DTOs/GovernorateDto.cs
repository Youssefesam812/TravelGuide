namespace Snap.APIs.DTOs
{
    public class GovernorateDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string ImageUrl { get; set; }
        public string Description { get; set; }
        public List<TopPlaceDto> TopPlaces { get; set; }
    }

    public class TopPlaceDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string ImageUrl { get; set; }
    }

}
