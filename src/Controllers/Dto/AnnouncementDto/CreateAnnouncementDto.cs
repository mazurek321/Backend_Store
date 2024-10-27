namespace projekt.src.Controllers.Dto.AnnouncementDto;

public record CreateAnnouncementDto
{
    public string Title { get; set; }
    public string? Description { get; set; }
    public int Amount { get; set; }
    public decimal Cost { get; set; }
    public List<string>? Categories { get; set; }
    public Dictionary<string, Dictionary<string,int>>? ColorsSizesAmount {get; set;}
    public Dictionary<string, int>? ColorsAmount {get; set;}
    public string? Model_Brand { get; set; }
}