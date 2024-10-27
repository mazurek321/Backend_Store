using System.Text.RegularExpressions;
using projekt.src.Exceptions;

namespace projekt.src.Models.Users;

public record PostCode
{
    public PostCode(string value){
        if(!checkValidation(value)) throw new CustomException("Invalid post code.");
        Value = value;
    }
    
    public string Value {get; }

    private bool checkValidation(string value){
        return Regex.IsMatch(value, @"^[0-9]{2}-[0-9]{3}");
    }
}