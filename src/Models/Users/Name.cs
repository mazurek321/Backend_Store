using projekt.src.Exceptions;

namespace projekt.src.Models.Users;

public record Name
{
    public Name(string value){
        if(string.IsNullOrWhiteSpace(value) || value.Length is > 50 or < 3) throw new CustomException("Invalid name.");
        Value = value;
    }
    
    public string Value {get; }
}