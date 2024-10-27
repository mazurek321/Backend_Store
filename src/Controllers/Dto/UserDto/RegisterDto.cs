using System.ComponentModel.DataAnnotations;
namespace projekt.src.Controllers.Dto.UserDto;

public record RegisterDto
{
    [Required(ErrorMessage = "Email is required.")]
    [EmailAddress]
    public string Email {get; set;}

    [Required(ErrorMessage = "Name is required.")]
    public string Name {get; set;}

    [Required(ErrorMessage = "Lastname is required.")]
    public string Lastname {get; set;}

    [Required(ErrorMessage = "Password is required.")]
    [Compare("ConfirmPassword", ErrorMessage ="Password does not match.")]
    [DataType(DataType.Password)]
    public string Password {get; set;}

    [Required(ErrorMessage = "Confirm password is required.")]
    [DataType(DataType.Password)]
    public string ConfirmPassword {get; set;}
    
    public string? Address {get; set;}
    public string? Location {get; set;}
    public string? PostCode {get; set;}
    public string? Phone {get; set;}
}