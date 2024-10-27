using projekt.src.Exceptions;

namespace projekt.src.Models.ShoppingCart;

public record Quantity
{
    public Quantity(int value)
    {
        if(value < 0) throw new CustomException("Invalid quantity of item.");
        Value = value;
    }

    public static Quantity operator +(Quantity a, Quantity b){
        if(a == null || b == null) throw new CustomException("Invalid quantity.");
        return new Quantity(a.Value + b.Value);
    }

    public int Value { get; }

    public int getValue() => Value;
}