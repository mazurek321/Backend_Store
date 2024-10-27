using System.ComponentModel.DataAnnotations;
namespace projekt.src.Controllers.Dto.UserDto;

public record ChangePasswordDto
{
    [Required(ErrorMessage = "Password incorrect.")]
    [DataType(DataType.Password)]
    public string OldPassword{get; set;}

    [Required(ErrorMessage = "Password is required.")]
    [Compare("ConfirmNewPassword", ErrorMessage ="Password does not match.")]
    [DataType(DataType.Password)]
    public string NewPassword {get; set;}

    [Required(ErrorMessage = "Confirm password is required.")]
    [DataType(DataType.Password)]
    public string ConfirmNewPassword {get; set;}
}