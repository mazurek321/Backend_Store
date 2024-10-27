namespace projekt.src.Models.ShoppingCart;

public class ShoppingCartItem
{
    public ShoppingCartItem(){}
    private ShoppingCartItem(Guid id, Guid shoppingCartId, Guid announcementId, Quantity quantity, string? color, string? size)
    {
        Id = id;
        ShoppingCartId = shoppingCartId;
        AnnouncementId = announcementId;
        Quantity = quantity;
        SelectedColor = color;
        SelectedSize = size;
    }

    public Guid Id { get; private set; }
    public Guid ShoppingCartId { get; private set; }
    public Guid AnnouncementId { get; private set; }
    public Quantity Quantity { get; private set; }
    public string? SelectedColor { get; private set; }
    public string? SelectedSize { get; private set; }

    public static ShoppingCartItem NewItem(Guid shoppingCartId, Guid announcementId, Quantity quantity, string? color, string? size)
    {
        return new ShoppingCartItem(Guid.NewGuid(), shoppingCartId, announcementId, quantity, color, size);
    }

    public void Update(string? selectedColor, string? selectedSize, Quantity quantity)
    {
        SelectedColor = selectedColor;
        SelectedSize = selectedSize;
        Quantity = quantity;
    }
}
