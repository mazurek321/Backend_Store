using projekt.src.Exceptions;

namespace projekt.src.Models.Store;

public record Cost
{        
    public Cost(decimal value){
        if(value < 0) throw new CustomException("Invalid cost.");
        Value = value;
    }

    public static Cost operator +(Cost a, Cost b)
    {
        if(a is null || b is null ) throw new CustomException("Invalid cost");
        return new Cost(a.Value + b.Value);
    }

    public static Cost operator*(Cost a, int b)
    {
        return new Cost(a.Value*b);
    }

    public decimal Value {get; }    
}
