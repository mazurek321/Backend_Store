namespace projekt.src.Models.ShoppingCart;

public class AddItemResult
{
    public ShoppingCartItem Item{ get; set; }
    public bool IsNew { get; set; }

    public AddItemResult(ShoppingCartItem item, bool isNew)
    {
        Item = item;
        IsNew = isNew; 
    }    
}
