using System.Security.Claims;
using Microsoft.EntityFrameworkCore;
using projekt.src.Data;
using projekt.src.Exceptions;
using projekt.src.Models.Users;

namespace projekt.src.Controllers;

public class UserService
{
    private readonly ApiDbContext _dbContext;

    public UserService(ApiDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<User> CurrentUser(ClaimsPrincipal userClaims)
    {
        var userId = userClaims.FindFirst("UserId").Value;
        var user = await _dbContext.Users.FirstOrDefaultAsync(x=>x.Id == userId); 
       
        if(user is null) throw new CustomException("User not found.");
        return user;   
    }

    public async Task<bool> CheckIfUSerExistsById(Guid userId)
    {
        var id = new UserId(userId);
        var user = await _dbContext.Users.AnyAsync(x=>x.Id == id); 
        return user;   
    }
    
}
