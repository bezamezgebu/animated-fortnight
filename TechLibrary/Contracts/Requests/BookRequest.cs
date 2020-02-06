namespace TechLibrary.Models
{
    public class BookRequest
    {
        public int BookId { get; set; }
        public string Title { get; set; }
        public string ISBN { get; set; }
        public string PublishedDate { get; set; }
        public string ThumbnailUrl { get; set; }
        public string Descr { get; set; }
    }
}
