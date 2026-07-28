using Domain.Entities;

namespace Api.src.Domain.Interfaces
{
    public interface IUserRepository
    {
        Task<bool> IsEmailUniqueAsync(string email);
        Task<Users?> GetByEmailAsync(string email);
        Task AddAsync(Users user);
        Task SaveChangesAsync();
    }
}
