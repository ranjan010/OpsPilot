namespace Api.src.Infrastructure.Repositories;
using Api.Infrastructure;
using Api.src.Application.Interfaces;
using Api.src.Domain.Interfaces;
using global::Domain.Entities;
using Microsoft.EntityFrameworkCore;

public class UserRepository : IUserRepository
{
    private readonly ApplicationDbContext _dbContext;

    public UserRepository(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<bool> IsEmailUniqueAsync(string email)
    {
        return !await _dbContext.Users.AnyAsync(u => u.Email == email);
    }

    public async Task<Users?> GetByEmailAsync(string email)
    {
        return await _dbContext.Users.FirstOrDefaultAsync(u => u.Email == email);
    }

    public async Task AddAsync(Users user)
    {
        await _dbContext.Users.AddAsync(user);
    }

    public async Task SaveChangesAsync()
    {
        await _dbContext.SaveChangesAsync();
    }
}