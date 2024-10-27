namespace projekt.src.Controllers.Dto.UserDto;

public record UserDto
{
    public string Id {get; set;}
    public string Email {get; set; }
    public string Name {get; set; }
    public string LastName {get; set; }
    public string? Address {get; set; }
    public string? Location {get; set; }
    public string? PostCode {get; set; }
    public string? Phone {get; set; }
    public string Role {get; set; }
    public string CreatedAt {get; set;}


}