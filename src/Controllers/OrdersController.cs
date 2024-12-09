using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using projekt.src.Controllers.Dto.OrdersDto;
using projekt.src.Data;
using projekt.src.Exceptions;
using projekt.src.Models.Orders;
using projekt.src.Models.ShoppingCart;
using projekt.src.Models.Store;
using projekt.src.Models.Users;


namespace projekt.src.Controllers;

[ApiController]
[Route("[controller]")]
public class OrdersController : ControllerBase
{
    private readonly ApiDbContext _dbContext;
    private readonly UserService _userService;

    public OrdersController(
        ApiDbContext dbContext,
        UserService userService
    )
    {
        _dbContext = dbContext;
        _userService = userService;
    }

[HttpPost]
[Authorize]
public async Task<IActionResult> CreateOrder(
    [FromBody] string deliveryMethod, bool order
)
{
    if (order)
    {
        var user = await _userService.CurrentUser(User);
        var cart = await _dbContext.ShoppingCarts
                            .Include(x => x.Items)
                            .FirstOrDefaultAsync(x => x.OwnerId == user.Id);

        if (cart is null) throw new CustomException("Cart not found.");
        if (!cart.Items.Any()) throw new CustomException("Cart is empty.");

        var delivery = new DeliveryMethod(deliveryMethod);
        List<UserId> owners = new List<UserId>();

        foreach (var item in cart.Items)
        {
            var announcement = await _dbContext.Announcements
                                               .Include(x => x.Item) 
                                               .FirstOrDefaultAsync(x => x.Id == item.AnnouncementId);
            if (announcement != null)
            {
                var owner = announcement.OwnerId;
                if (!owners.Contains(owner)) owners.Add(owner);

                if (announcement.Item.Amount.Value >= item.Quantity.Value)
                {
                    var new_amount = new ItemAmount(announcement.Item.Amount.Value - item.Quantity.Value);

                        if (item.SelectedColor is not null && item.SelectedSize is not null)
                        {
                            var tab = announcement.Item.ColorsSizesAmounts;
                            
                            
                            tab[item.SelectedColor][item.SelectedSize] -= item.Quantity.Value;

                            announcement.Item.UpdateItem(
                                announcement.Item.Title,
                                announcement.Item.Description,
                                new_amount,
                                announcement.Item.Cost,
                                announcement.Item.Categories,
                                tab,
                                announcement.Item.ColorsAmount,
                                announcement.Item.Model_Brand
                            );

                            _dbContext.Update(announcement.Item);

                        }
                    
                        else if(item.SelectedColor is not null){
                            var tab = announcement.Item.ColorsAmount;
                            tab[item.SelectedColor] -= item.Quantity.Value;

                            announcement.Item.UpdateItem(
                                announcement.Item.Title,
                                announcement.Item.Description,
                                new_amount,
                                announcement.Item.Cost,
                                announcement.Item.Categories,
                                announcement.Item.ColorsSizesAmounts,
                                tab,
                                announcement.Item.Model_Brand
                            );

                            _dbContext.Update(announcement.Item);
                        }
                        else{
                            announcement.Item.UpdateItem(
                                announcement.Item.Title,
                                announcement.Item.Description,
                                new_amount,
                                announcement.Item.Cost,
                                announcement.Item.Categories,
                                announcement.Item.ColorsSizesAmounts,
                                announcement.Item.ColorsAmount,
                                announcement.Item.Model_Brand
                            );

                             _dbContext.Update(announcement.Item);
                        }
                }
                else
                {
                    throw new CustomException($"Not enough stock for item: {announcement.Item.Title}");
                }
            }
        }

        var newOrder = Orders.New(
            user.Id, owners, DateTime.UtcNow, cart.Items, delivery
        );

        await _dbContext.Orders.AddAsync(newOrder);
        cart.ClearCart();
        await _dbContext.SaveChangesAsync();

        return Ok(newOrder);
    }

    return NoContent();
}


    [HttpGet("my-orders")]
    [Authorize]
    public async Task<IActionResult> GetMyOrders()
    {
        var user = await _userService.CurrentUser(User);

        var cartHistories = await _dbContext.Orders
                                .Include(x => x.Items)
                                .Where(x => x.OrderingPerson == user.Id)
                                .ToListAsync();

        return Ok(cartHistories);
    }

    [HttpGet]
    [Authorize]
    public async Task<List<OrderDto>> GetOrdersForMe()
    {
        var user = await _userService.CurrentUser(User);

        var ownedAnnouncementsId = await _dbContext.Announcements
                                                .Where(x=>x.OwnerId == user.Id)
                                                .Select(x=>x.Id)
                                                .ToListAsync();

        var orders = await _dbContext.Orders
                            .Include(x=>x.Items)
                            .Where(x=>x.Items.Any(i=> ownedAnnouncementsId.Contains(i.AnnouncementId)))
                            .ToListAsync();

        var filtered = orders.Select(
                            order=> new OrderDto{
                                Id = order.Id,
                                OrderingPerson = order.OrderingPerson,
                                OrderedAt = order.OrderedAt,
                                Items = order.Items.Where(item => ownedAnnouncementsId.Contains(item.AnnouncementId)).ToList(),
                                DeliveryMethod = order.DeliveryMethod
                            }
        ).ToList();


        // var procedure = _dbContext.Orders.FromSqlRaw($"CALL getordersforme('{user.Id.Value}'::UUID)").AsAsyncEnumerable();

        var procedure = await _dbContext.Set<User>()
                                        .FromSqlRaw($"CALL getordersforme('{user.Id.Value}'::UUID)")
                                        .ToListAsync();

        Console.WriteLine(procedure);
        Console.WriteLine("procedureeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeee");

        return filtered;
    }

    [HttpPut]
    [Authorize]
    public async Task<IActionResult> UpdateOrderStatus([FromQuery] Guid orderId, Guid itemId, [FromBody] string orderStatus)
    {
        var orders = await GetOrdersForMe();

        var order = orders
            .FirstOrDefault(x=>x.Id == orderId);

        if(order is null) throw new CustomException("Order not found.");
            
        var status = new OrderStatus(orderStatus);
        
        var item = order.Items.FirstOrDefault(x=>x.Id == itemId);
        item.UpdateItemOrderStatus(status);

        await _dbContext.SaveChangesAsync();
        
        return Ok(order);
    }

    
}
