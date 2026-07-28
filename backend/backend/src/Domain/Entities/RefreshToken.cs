using Domain.Common;

namespace Domain.Entities;

public class RefreshToken : BaseEntity
{
    public Guid UserId { get; set; }
    public Users User { get; set; } = default!;

    public string Token { get; set; } = string.Empty;
    public DateTime ExpiresAt { get; set; }
    public bool IsRevoked { get; set; } = false;
}
