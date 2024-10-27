using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using projekt.src.Controllers.Dto.ReviewDto;
using projekt.src.Data;
using projekt.src.Exceptions;
using projekt.src.Models.Reviews;
using projekt.src.Models.Users;


namespace projekt.src.Controllers;

[ApiController]
[Route("[controller]")]
public class ReviewsController : ControllerBase
{
    private readonly ApiDbContext _dbContext;
    private readonly UserService _userService;

    public ReviewsController(
        ApiDbContext dbContext,
        UserService userService
    )
    {
        _dbContext = dbContext;
        _userService = userService;
    }

    [HttpPost]
    [Authorize]
    public async Task<IActionResult> Create(
        [FromQuery] Guid? announcementId, Guid? UserId,
        [FromBody] CreateReviewDto dto    
    )
    {
        var user = await _userService.CurrentUser(User);
        
        if(announcementId is not null)
        {
            var announcement = await _dbContext.Announcements.FirstOrDefaultAsync(x=>x.Id == announcementId);
            if(announcement is null) throw new CustomException("Announcement not found.");
            
            var existingReview = await _dbContext.Reviews.AnyAsync(x=>x.AnnouncementId == announcementId && x.OwnerId == user.Id);
            if(existingReview) throw new CustomException("You already gave a review to this announcement.");
        }
        
        var rating = new Rating(dto.Rating);
        var userId = new UserId(UserId.Value);

        if(UserId is not null)
        {
            var reviewdUser = await _userService.CheckIfUSerExistsById(UserId.Value);
            if(!reviewdUser) throw new CustomException("User not found.");

            var existingReview = await _dbContext.Reviews.AnyAsync(x=>x.UserId == userId && x.OwnerId == user.Id);
            if(existingReview) throw new CustomException("You already gave a review to this user.");
        }
        
        var review = Reviews.NewReview(
                announcementId.Value,
                userId,
                user.Id,
                dto.Comment,
                rating
        );

        await _dbContext.Reviews.AddAsync(review);
        await _dbContext.SaveChangesAsync();

        return Ok(review);
    }

    [HttpGet]
    public async Task<IActionResult> GetReviews(
        [FromQuery] Guid? reviewId,
        [FromQuery] Guid? userId,
        [FromQuery] Guid? announcementId
    )
    {
        var reviews = await _dbContext.Reviews.ToListAsync(); 

        if(reviewId is not null){
            var review = await _dbContext.Reviews.FirstOrDefaultAsync(x=>x.Id == reviewId);
            if(review is null) throw new CustomException("Review not found.");
            return Ok(review);
        }

        if(userId is not null)
        {
            var user = await _userService.CheckIfUSerExistsById(userId.Value);
            if(!user) throw new CustomException("User not found.");
            var id = new UserId(userId.Value);
            reviews = await _dbContext.Reviews.Where(x=>x.OwnerId==id).ToListAsync();
        }

        if(announcementId is not null)
        {
            var announcement = await _dbContext.Announcements.AnyAsync(x=>x.Id==announcementId);
            if(!announcement) throw new CustomException("Announcement not found.");
            reviews = await _dbContext.Reviews.Where(x=>x.AnnouncementId == announcementId).ToListAsync();
        }

        return Ok(reviews);
    }

    [HttpDelete]
    [Authorize]
    public async Task<IActionResult> Delete([FromQuery] Guid reviewId)
    {
        var review = await _dbContext.Reviews.FirstOrDefaultAsync(x=>x.Id == reviewId);
        if(review is null) throw new CustomException("Review not found.");

        _dbContext.Remove(review);
        await _dbContext.SaveChangesAsync();
        return NoContent();
    }
    
}
