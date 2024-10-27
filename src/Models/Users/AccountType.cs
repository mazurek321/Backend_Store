using projekt.src.Exceptions;

namespace projekt.src.Models.Users;

public record AccountType
{

    public AccountType(string value){
        if(string.IsNullOrWhiteSpace(value) || !AvailableRoles.Contains(value.ToLower())) throw new CustomException("Invalid account type.");
        Value = value;
    }

    public string Value { get; }
    private static readonly List<string> AvailableRoles = new List<string> {"admin", "user"};

    public static AccountType Admin(){
        return new AccountType("admin");
    }

    public static AccountType User(){
        return new AccountType("user");
    }
}