using System.ComponentModel.DataAnnotations;
namespace projekt.src.Controllers.Dto.UserDto;

public record LoginDto
{
    [Required(ErrorMessage = "Email is required.")]
    [EmailAddress]
    public string Email {get; set;}
    
    [Required(ErrorMessage = "Password is required.")]
    [DataType(DataType.Password)]
    public string Password {get; set;}
}