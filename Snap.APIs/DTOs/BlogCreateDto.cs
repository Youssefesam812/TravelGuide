namespace Snap.APIs.DTOs
{
    public class BlogCreateDto
    {
        public string Blog { get; set; }
        public string UserId { get; set; }
    }

    public class BlogDto
    {
        public int Id { get; set; }
        public string Blog { get; set; }
        public string UserId { get; set; }
    }
}
