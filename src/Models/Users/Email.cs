using System.Text.RegularExpressions;
using projekt.src.Exceptions;

namespace projekt.src.Models.Users;

public record Email
{
    public Email(string value){
        if(string.IsNullOrEmpty(value) || !checkValidation(value)) throw new CustomException("Invalid email.");
        Value = value;
    }

    public string Value {get; }

    private bool checkValidation(string value){
        return Regex.IsMatch(value,
            @"^([\w-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([\w-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$" );
    }
}