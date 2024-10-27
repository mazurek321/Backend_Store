using projekt.src.Exceptions;

namespace projekt.src.Models.Users;

public record Password
{
    public Password(string value){
        if(string.IsNullOrWhiteSpace(value) || value.Length is > 200 or < 5) throw new CustomException("Invalid id.");
        Value = value;
    }
    
    public string Value {get; }
}