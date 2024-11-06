using projekt.src.Models.Orders;
using projekt.src.Models.Users;

namespace projekt.src.Controllers.Dto.OrdersDto;

public record OrderDto
{
    public Guid Id { get; set; }
    public UserId OrderingPerson { get; set; }
    public DateTime OrderedAt { get; set; }
    public List<OrderedItems> Items { get; set; }
    public DeliveryMethod DeliveryMethod { get; set; }
}