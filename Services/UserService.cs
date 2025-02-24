using CodeFirstJWT1.Models;
using Microsoft.EntityFrameworkCore;
namespace CodeFirstJWT1.Services;

public class UserService
{
    private readonly CodeFirstJWTDbContext _dbContext;

    public UserService(CodeFirstJWTDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<User> GetUserByUsername(string username)
    {
        return await _dbContext.Users.FirstOrDefaultAsync(u => u.Username == username);
    }

    public async Task<User> AddUser(User user)
    {
        _dbContext.Users.Add(user);
        await _dbContext.SaveChangesAsync();
        return user;
    }
}