using projekt.src.Models.ShoppingCart;

namespace projekt.src.Models.Orders;

public class OrderedItems
{
    public OrderedItems(){}
    private OrderedItems(Guid id, Guid orderId, Guid shoppingCartId, Guid announcementId, Quantity quantity, string? color, string? size)
    {
        Id = id;
        OrderId = orderId;
        ShoppingCartId = shoppingCartId;
        AnnouncementId = announcementId;
        Quantity = quantity;
        SelectedColor = color;
        SelectedSize = size;
    }

    public Guid Id { get; private set; }
    public Guid OrderId { get; private set; }
    public Guid ShoppingCartId { get; private set; }
    public Guid AnnouncementId { get; private set; }
    public Quantity Quantity { get; private set; }
    public string? SelectedColor { get; private set; }
    public string? SelectedSize { get; private set; }

    public static OrderedItems NewItem(Guid shoppingCartId, Guid orderId, Guid announcementId, Quantity quantity, string? color, string? size)
    {
        return new OrderedItems(Guid.NewGuid(), orderId, shoppingCartId, announcementId, quantity, color, size);
    }

    public static List<OrderedItems> ConvertToOrdered(List<ShoppingCartItem> items, Guid orderId)
    {
        var orderedItems = new List<OrderedItems>();

        foreach(var item in items)
        {
            var orderedItem = NewItem(
                item.ShoppingCartId, 
                orderId,
                item.AnnouncementId,
                item.Quantity,
                item.SelectedColor,
                item.SelectedSize
            );

            orderedItems.Add(orderedItem);
        }

        return orderedItems;
    }
}
