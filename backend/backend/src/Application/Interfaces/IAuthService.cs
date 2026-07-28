using Domain.Entities;

namespace Api.src.Application.Interfaces
{
    public interface IAuthService
    {
        Task<Users?> ValidateUserAsync(string email, string password, CancellationToken cancellationToken = default);
    }
}
