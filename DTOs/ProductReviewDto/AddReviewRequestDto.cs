namespace SimpleShop.DTOs
{
    public class AddReviewRequestDto
    {
        public int ProductId { get; set; }
        public int Rating { get; set; }
        public string Comment { get; set; } = string.Empty;
        public List<IFormFile> Images { get; set; } = new();
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }

}