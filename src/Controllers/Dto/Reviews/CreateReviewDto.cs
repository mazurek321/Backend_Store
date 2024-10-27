namespace projekt.src.Controllers.Dto.ReviewDto;

public record CreateReviewDto
{
    public string? Comment { get; set; }
    public int Rating { get; set; }

}