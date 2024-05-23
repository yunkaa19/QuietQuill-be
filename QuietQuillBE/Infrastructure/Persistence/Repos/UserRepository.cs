using Microsoft.EntityFrameworkCore;
using Domain.Entities;
using Domain.Exceptions.Users;
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
        return user ?? throw new UserNotFoundException(userId);
    }

    public async Task AddAsync(User user)
    {
        if (user.IdentityID == null)
        {
            throw new ArgumentException("IdentityID cannot be zero.");
        }

        _dbContext.Users.Add(user);
        await _dbContext.SaveChangesAsync();
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
    
    public async Task<User> GetUserByEmail(string email)
    {
        var user = await _dbContext.Users.FirstOrDefaultAsync(u => u.Email == email);
        if (user == null)
        {
            throw new UserNotFoundException(new Guid());
        }
        return await _dbContext.Users.FirstOrDefaultAsync(u => u.Email == email);
    }
}