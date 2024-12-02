using Microsoft.VisualBasic;
using projekt.src.Models.ShoppingCart;

namespace projekt.src.Models.Store;

public class Item
{
    private Item(){}

    private Item(string title, string? description, ItemAmount amount, Cost cost, List<string>? categories, 
                 Dictionary<string, Dictionary<string, int>>? colorsSizesAmount, Dictionary<string, int>? colorsAmount,
                 string model_Brand)
    {
        Title = title;
        Description = description;
        Amount = amount;
        Cost = cost;
        Categories = categories;
        ColorsSizesAmounts = colorsSizesAmount;
        ColorsAmount = colorsAmount;
        Model_Brand = model_Brand;
    }

    public string Title { get; private set; }
    public string? Description { get; private set; }
    public ItemAmount Amount { get; private set; }
    public Cost Cost { get; private set; }
    public List<string>? Categories {get; private set; }
    public Dictionary<string, Dictionary<string, int>>? ColorsSizesAmounts {get; private set;}
    public Dictionary<string, int>? ColorsAmount { get; private set; }
    public string? Model_Brand { get; private set; }
    public DateTime? UpdatedAt { get; private set; }

    public static Item NewItem(
        string title, string? description, ItemAmount amount, Cost cost, List<string>? categories, 
        Dictionary<string, Dictionary<string,int>>? colorsSizesAmount, Dictionary<string, int>? colorsAmount,
        string model_Brand)
    {
        return new Item(title, description, amount, cost, categories, colorsSizesAmount, colorsAmount, model_Brand);
    }

    public void UpdateItem(
        string? title, string? description, ItemAmount? amount, Cost? cost, List<string>? categories, 
        Dictionary<string, Dictionary<string,int>>? colorsSizesAmount, Dictionary<string, int>? colorsAmount,
        string? model_Brand
    )
    {
        Title = title;
        Description = description;
        Amount = amount;
        Cost = cost;
        Categories = categories;
        ColorsSizesAmounts = colorsSizesAmount;
        ColorsAmount = colorsAmount;
        Model_Brand = model_Brand;
        UpdatedAt = DateTime.UtcNow;
    }

}
