using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using projekt.src.Data;
using projekt.src.Exceptions;
using projekt.src.Models.SavedAnnouncements;

namespace projekt.src.Controllers;

[ApiController]
[Route("[controller]")]
public class SavedAnnouncementsController : ControllerBase
{
    private readonly ApiDbContext _dbContext;
    private readonly UserService _userService;

    public SavedAnnouncementsController(
        ApiDbContext dbContext,
        UserService userService
    )
    {
        _dbContext = dbContext;
        _userService = userService;
    }

    [HttpPost]
    [Authorize]
    public async Task<IActionResult> SaveAnnouncement([FromQuery] Guid announcementId)
    {
        var exists = await _dbContext.Announcements.AnyAsync(x=>x.Id == announcementId);
        if(!exists) throw new CustomException("Announcement not found.");

        var user = await _userService.CurrentUser(User);
        
        var alreadySaved = await _dbContext.SavedAnnouncements.AnyAsync(x=>x.UserId == user.Id && x.AnnouncementId == announcementId);
        if(alreadySaved) throw new CustomException("You already saved this announcement.");

        var save = SavedAnnouncements.SaveAnnouncement(user.Id, announcementId); 

        _dbContext.SavedAnnouncements.Add(save);
        await _dbContext.SaveChangesAsync();

        return Ok(save);
    }

    [HttpGet]
    [Authorize]
    public async Task<IActionResult> GetSavedAnnouncements()
    {
        var user = await _userService.CurrentUser(User);
        var saved = await _dbContext.SavedAnnouncements.Where(x=>x.UserId==user.Id).ToListAsync();
        return Ok(saved);
    }

    [HttpDelete]
    [Authorize]
    public async Task<IActionResult> DeleteSavedAnnouncement([FromQuery] Guid announcementId)
    {
        var exists = await _dbContext.Announcements.AnyAsync(x=>x.Id == announcementId);
        if(!exists) throw new CustomException("Announcement not found.");

        var user = await _userService.CurrentUser(User);
        var saved = await _dbContext.SavedAnnouncements.FirstOrDefaultAsync(x=>x.UserId==user.Id && x.AnnouncementId == announcementId);
        if(saved is null) throw new CustomException("Saved announcement not found.");

        _dbContext.SavedAnnouncements.Remove(saved);
        await _dbContext.SaveChangesAsync();

        return NoContent();
    }

    
}
