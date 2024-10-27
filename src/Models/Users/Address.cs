using projekt.src.Exceptions;

namespace projekt.src.Models.Users;

public record Address
{
    public Address(string value){
        if(value.Length is > 200 or < 3) throw new CustomException("Invalid address.");
        Value = value;
    }
    
    public string Value {get; }
}