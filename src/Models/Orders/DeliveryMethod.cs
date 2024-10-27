using projekt.src.Exceptions;

namespace projekt.src.Models.Orders;

public record DeliveryMethod
{
    public DeliveryMethod(string value)
    {
        if(string.IsNullOrEmpty(value) || !DeliveryMethods.Contains(value.ToLower())) throw new CustomException("Invalid delivery method.");
        Value = value;
    }
    public string Value { get; }
    private static readonly List<string> DeliveryMethods = new List<string>{"inpost", "dpd"};
}