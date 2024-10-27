using projekt.src.Exceptions;

namespace projekt.src.Models.Store;

public record ItemAmount
{    
    public ItemAmount(int value)
    {
        if(value < 0) throw new CustomException("Invalid amount.");
        Value = value;
    }
    public int Value { get; }
    
}