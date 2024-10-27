namespace projekt.src.Controllers.Dto.UserDto;

public record ChangeInformationDto
{
    public string? Name { get; set;}
    public string? LastName { get; set;}
    public string? Address { get; set;}
    public string? Location { get; set;}
    public string? PostCode { get; set;}
    public string? Phone { get; set;}
}