namespace ChatKid.OpenAI.ImageGeneration
{
    public class ImageModel
    {
        public DateTime CreatedDate { get; set; }
        public List<ImageUrl> ImageUrls;
    }
    public record ImageUrl { 
        public string Url { get; set; }
    }
}
