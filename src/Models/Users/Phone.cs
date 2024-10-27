using projekt.src.Exceptions;

namespace projekt.src.Models.Users;

public record Phone
{
    public Phone(string value){
        if(value.Length is > 15 or < 8) throw new CustomException("Invalid phone number.");
        Value = value;
    }

    public string Value {get; }
}
