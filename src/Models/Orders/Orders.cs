using projekt.src.Models.ShoppingCart;
using projekt.src.Models.Users;

namespace projekt.src.Models.Orders;

public class Orders
{    
    private Orders(){}
    
    private Orders(Guid id, UserId orderingPerson, List<UserId> ownersId, DateTime orderedAt,List<OrderedItems> items, DeliveryMethod deliveryMethod, OrderStatus orderStatus)
    {
        Id = id;
        OrderingPerson = orderingPerson;
        OwnersId = ownersId;
        OrderedAt = orderedAt;
        Items = items;
        DeliveryMethod = deliveryMethod;
        OrderStatus = orderStatus;

    }

    public Guid Id { get; private set; }
    public UserId OrderingPerson { get; private set; }
    public List<UserId> OwnersId { get; private set; }
    public DateTime OrderedAt { get; private set; }
    public List<OrderedItems> Items { get; private set; }
    public DeliveryMethod DeliveryMethod { get; private set; }
    public OrderStatus OrderStatus { get; private set; }
    

    public static Orders New(UserId orderingPerson, List<UserId> ownersId, DateTime orderedAt, List<ShoppingCartItem> items, DeliveryMethod deliveryMethod)
    {
        var orderId = Guid.NewGuid();
        var orderedItems = OrderedItems.ConvertToOrdered(items, orderId);
        return new Orders(orderId, orderingPerson, ownersId, orderedAt, orderedItems, deliveryMethod, OrderStatus.Pending());
    }

    public void BeginTransaction()
    {
        
    }
}
