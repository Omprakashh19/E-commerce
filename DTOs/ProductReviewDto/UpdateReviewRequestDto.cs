namespace SimpleShop.DTOs
{
    public class UpdateReviewRequestDto
    {
        public int ProductId { get; set; }
        public string Comment { get; set; } = string.Empty;
        public int Rating { get; set; } 
        public List<IFormFile> NewImages { get; set; } = new();
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

    }
}