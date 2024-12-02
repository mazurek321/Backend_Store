using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using projekt.src.Controllers.Dto.ShoppingCartDto;
using projekt.src.Data;
using projekt.src.Exceptions;
using projekt.src.Models.ShoppingCart;

namespace projekt.src.Controllers;

[ApiController]
[Route("[controller]")]
public class ShoppingCartController : ControllerBase
{
    private readonly ApiDbContext _dbContext;
    private readonly UserService _userService;

    public ShoppingCartController(
        ApiDbContext dbContext,
        UserService userService
    )
    {
        _dbContext = dbContext;
        _userService = userService;
    }

    [NonAction]
    public async Task <ShoppingCart> CreateCart()
    {
        var user = await _userService.CurrentUser(User);
        var cartExists = await GetCart();

        if(cartExists is null){
            var cart = ShoppingCart.New(user.Id);

            _dbContext.ShoppingCarts.Add(cart);
            await _dbContext.SaveChangesAsync();
            
            return cart;
        }

        return cartExists;
    }

    [HttpPost]
[Authorize]
public async Task<IActionResult> AddItem(
    [FromQuery] Guid AnnouncementId,
    [FromBody] AddItemDto dto
)
{
    var announcement = await _dbContext.Announcements.FirstOrDefaultAsync(x => x.Id == AnnouncementId);
    if (announcement is null)
        throw new CustomException("Announcement not found.");

    var user = await _userService.CurrentUser(User);
    var cart = await CreateCart();
    if (cart is null)
        throw new CustomException("Cart not found.");

    var quantity = new Quantity(dto.Quantity);

    if(dto.Quantity <= 0) throw new CustomException("Invalid amount.");

    var totalAvailableQuantity = announcement.Item.Amount.Value;
    if (quantity.Value > totalAvailableQuantity)
        throw new CustomException("The selected quantity exceeds the available amount of the item.");

    if (dto.SelectedColor != null && announcement.Item.ColorsAmount != null)
    {
        var availableColorAmount = announcement.Item.ColorsAmount
            .FirstOrDefault(x => x.Key == dto.SelectedColor).Value;

        if (quantity.Value > availableColorAmount)
            throw new CustomException($"The selected quantity for color {dto.SelectedColor} exceeds the available amount.");
    }

    if (dto.SelectedColor != null && dto.SelectedSize != null && announcement.Item.ColorsSizesAmounts != null)
    {
        var availableSizeAmount = announcement.Item.ColorsSizesAmounts
            .FirstOrDefault(x => x.Key == dto.SelectedColor).Value
            .FirstOrDefault(y => y.Key == dto.SelectedSize).Value;

        if (quantity.Value > availableSizeAmount)
            throw new CustomException($"The selected quantity for size {dto.SelectedSize} of color {dto.SelectedColor} exceeds the available amount.");
    }

    var cartItem = cart.AddItem(AnnouncementId, quantity, dto.SelectedColor, dto.SelectedSize);
    if (cartItem is null)
        throw new CustomException("Item not found.");

    _dbContext.ShoppingCartItems.AddAsync(cartItem);
    await _dbContext.SaveChangesAsync();

    return Ok(cart);
}


    [HttpPut]
    [Authorize]
    public async Task <IActionResult> UpdateItem(
        [FromQuery] Guid ItemId,
        [FromBody] AddItemDto dto)
    {
        var user = await _userService.CurrentUser(User);
        var cart = await GetCart();
        
        var existingItem = cart.Items.FirstOrDefault(x=>x.Id == ItemId);

        if(existingItem is null) throw new CustomException("Item not found.");

        var quantity = new Quantity(dto.Quantity);
        cart.UpdateItem(ItemId, quantity, dto.SelectedColor, dto.SelectedSize);

        await _dbContext.SaveChangesAsync();

        return Ok(cart);
    }


    [HttpGet]
    [Authorize]
    public async Task <ShoppingCart> GetCart()
    {
        var user = await _userService.CurrentUser(User);
        var cart = await _dbContext.ShoppingCarts
                            .Include(x=>x.Items)
                            .FirstOrDefaultAsync(x=>x.OwnerId == user.Id);

        return cart;
    }

    [HttpDelete("clear")]
    [Authorize]
    public async Task<IActionResult> ClearCart()
    {
        var cart = await GetCart();

        if(cart is null) throw new CustomException("Cart not found.");

        _dbContext.ShoppingCartItems.RemoveRange(cart.Items);
        cart.ClearCart();
        await _dbContext.SaveChangesAsync();

        return Ok();

    }

    [HttpDelete]
    [Authorize]
    public async Task<IActionResult> DeleteCart([FromQuery] Guid? itemId)
    {
        var user = await _userService.CurrentUser(User);
        var cart = await GetCart();

        if(cart.OwnerId != user.Id) throw new CustomException("You dont have permission to delete this cart.");
        if(cart is null) throw new CustomException("Cart not found.");

        if(itemId is not null){
            var announcement = await _dbContext.Announcements
                        .Where(x=>x.Id == itemId)
                        .FirstOrDefaultAsync();
            if(announcement is null) throw new CustomException("Item not found.");

             cart.RemoveOneItem(announcement);
            
        }else{
            cart.ClearCart();
            _dbContext.ShoppingCartItems.RemoveRange(cart.Items);
            _dbContext.ShoppingCarts.Remove(cart);
        }
        await _dbContext.SaveChangesAsync();

        return NoContent();
    }
}
