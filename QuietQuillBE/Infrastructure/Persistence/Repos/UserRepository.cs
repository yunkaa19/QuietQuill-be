using Microsoft.EntityFrameworkCore;
using Domain.Entities;
using Domain.Repositories;

namespace Infrastructure.Persistence.Repos;


public  class UserRepository : IUserRepository
{
    private readonly ApplicationDbContext _dbContext;

    public UserRepository(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<User> GetByIdAsync(Guid userId)
    {
        var user = await _dbContext.Users.FindAsync(userId);
        if (user == null)
        {
            throw new Exception($"No user found with ID {userId}");
        }
        return user;
    }

    public async Task<bool> AddAsync(User user)
    {
        var existingUser = await _dbContext.Users.FindAsync(user.Email);
        if (existingUser != null)
        {
            _dbContext.Users.Add(user);
            return true;
        }
        return false;
    }

    public async Task UpdateAsync(User user)
    {
        _dbContext.Users.Update(user);
        await _dbContext.SaveChangesAsync();
    }

    public async Task DeleteAsync(User user)
    {
        _dbContext.Users.Remove(user);
        await _dbContext.SaveChangesAsync();
    }

    public async Task<bool> UserExists(string email)
    {
        return await _dbContext.Users.AnyAsync(u => u.Email == email);
    }
}