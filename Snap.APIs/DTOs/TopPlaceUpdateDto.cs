namespace Snap.APIs.DTOs
{
    public class TopPlaceUpdateDto
    {
        public int Id { get; set; } // 0 for new places
        public string Name { get; set; }
        public string ImageUrl { get; set; }
    }
}
