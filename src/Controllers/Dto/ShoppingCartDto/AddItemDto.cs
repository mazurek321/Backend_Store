namespace projekt.src.Controllers.Dto.ShoppingCartDto;

public record AddItemDto
{
    public int Quantity { get; set; }
    public string? SelectedColor { get; set; }
    public string? SelectedSize { get; set; }
}