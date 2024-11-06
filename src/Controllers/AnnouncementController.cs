using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using projekt.src.Controllers.Dto.AnnouncementDto;
using projekt.src.Data;
using projekt.src.Exceptions;
using projekt.src.Models.Store;
using projekt.src.Models.Users;

namespace projekt.src.Controllers;

[ApiController]
[Route("[controller]")]
public class AnnouncementController : ControllerBase
{
    private readonly ApiDbContext _dbContext;
    private readonly UserService _userService;

    public AnnouncementController(
        ApiDbContext dbContext,
        UserService userService
    )
    {
        _dbContext = dbContext;
        _userService = userService;
    }

    [HttpPost]
    [Authorize]
    public async Task<IActionResult> CreateAnnouncement([FromBody] CreateAnnouncementDto dto)
    {
        var user = await _userService.CurrentUser(User);

        var itemAmount = new ItemAmount(dto.Amount);
        var cost = new Cost(dto.Cost);

        if(dto.ColorsSizesAmount is not null){
            var totalAmountInSizesAndColors = dto.ColorsSizesAmount.Sum(
                                                x=>x.Value.Sum(y=>y.Value)
                                            ); 
            if(dto.Amount != totalAmountInSizesAndColors) throw new CustomException("The amount in sizes and colors does not match the amount of object.");
        }

        if(dto.ColorsAmount is not null){
            var totalColorsAmount = dto.ColorsAmount.Sum(x=>x.Value);
            if(dto.Amount != totalColorsAmount) throw new Exception("The amount of colors does not match the amount of object.");
        }

        var item = Item.NewItem(
            dto.Title,
            dto.Description,
            itemAmount,
            cost,
            dto.Categories,
            dto.ColorsSizesAmount,
            dto.ColorsAmount,
            dto.Model_Brand
        );

        var announcement = Announcement.New(
            user.Id,
            item
        );

        _dbContext.Announcements.AddAsync(announcement);
        await _dbContext.SaveChangesAsync();

        return Ok(announcement);
    }

    [HttpGet("my")]
    [Authorize]
    public async Task<IActionResult> GetMyAnnouncements()
    {
        var user = await _userService.CurrentUser(User);
        var announcements = await _dbContext.Announcements
                                        .Where(x=>x.OwnerId == user.Id)
                                        .ToListAsync();

        return Ok(announcements);
    }

    [HttpGet]
    public async Task<IActionResult> GetAnnouncements(
        [FromQuery] Guid? userId,
        [FromQuery] Guid? announcementId
    )
    {
        var announcements = await _dbContext.Announcements.ToListAsync();

        if(userId is not null){
            var exists = await _userService.CheckIfUSerExistsById(userId.Value);
            if(!exists) throw new CustomException("User does not exist.");

            var id = new UserId(userId.Value);
            announcements = await _dbContext.Announcements
                                        .Where(x=>x.OwnerId == id)
                                        .ToListAsync();
        }

        if(announcementId is not null){
            announcements = await _dbContext.Announcements
                                        .Where(x=>x.Id == announcementId)
                                        .ToListAsync();

            if(announcements is null) throw new CustomException("Announcement not found.");
        }

        return Ok(announcements);
    }

    [HttpPut]
    [Authorize]
    public async Task <IActionResult> Update(
        [FromQuery] Guid itemId,
        [FromBody] UpdateAnnouncementDto dto
    )
    {
        var announcement = await _dbContext.Announcements.FirstOrDefaultAsync(x=>x.Id==itemId);
        if(announcement is null) throw new CustomException("Announcement not found.");

        var user = await _userService.CurrentUser(User);
        if(announcement.OwnerId != user.Id) throw new CustomException("You dont have persmission to update this announcement.");

        if(dto.ColorsSizesAmount is not null){
            var totalAmountInSizesAndColors = dto.ColorsSizesAmount.Sum(
                                                x=>x.Value.Sum(y=>y.Value)
                                            ); 
            if(dto.Amount != totalAmountInSizesAndColors) throw new CustomException("The amount in sizes and colors does not match the amount of object.");
        }

        if(dto.ColorsAmount is not null){
            var totalColorsAmount = dto.ColorsAmount.Sum(x=>x.Value);
            if(dto.Amount != totalColorsAmount) throw new Exception("The amount of colors does not match the amount of object.");
        }

        var itemAmount = new ItemAmount(dto.Amount.Value);
        var cost = new Cost(dto.Cost.Value);

        announcement.Item.UpdateItem(
            dto.Title,
            dto.Description,
            itemAmount,
            cost,
            dto.Categories,
            dto.ColorsSizesAmount,
            dto.ColorsAmount,
            dto.Model_Brand
        );

        await _dbContext.SaveChangesAsync();

        return Ok(announcement);
    }

    [HttpDelete]
    [Authorize]
    public async Task<IActionResult> Delete([FromQuery] Guid announcementId)
    {
        var announcement = await _dbContext.Announcements.FirstOrDefaultAsync(x=>x.Id == announcementId);
        if(announcement is null) throw new CustomException("Announcement not found.");

        var user = await _userService.CurrentUser(User);
        if(announcement.OwnerId != user.Id) throw new CustomException("You dont have permission to delete this announcement.");
        
        _dbContext.Remove(announcement);
        await _dbContext.SaveChangesAsync();

        return NoContent();
    }
}
