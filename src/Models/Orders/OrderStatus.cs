using projekt.src.Exceptions;

namespace projekt.src.Models.Orders;

public record OrderStatus
{
    public OrderStatus(string value)
    {
        if(string.IsNullOrEmpty(value) || !AvailableOrderStatus.Contains(value.ToLower())) throw new CustomException("Invalid delivery method.");
        Value = value;
    }
    public string Value { get; }
    private static readonly List<string> AvailableOrderStatus = new List<string>{"pending", "accepted", "sent"};

    public static OrderStatus Pending()
    {
        return new OrderStatus("pending");
    }

    public static OrderStatus Accepted()
    {
        return new OrderStatus("accepted");
    }

    public static OrderStatus Sent()
    {
        return new OrderStatus("sent");
    }
    
}