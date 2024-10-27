using projekt.src.Exceptions;

namespace projekt.src.Models.Users;

public record UserId
{
    public UserId(Guid value){
        if(value == Guid.Empty) throw new CustomException("Invalid id.");
        Value = value;
    }
    
    public static implicit operator UserId (string value){
        if(string.IsNullOrEmpty(value)) throw new CustomException("Invalid id.");
        return new UserId(Guid.Parse(value));
    }

    public Guid Value {get; }
}