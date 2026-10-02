namespace WaveApp.Core.Entities;

public class RefreshToken
{
    public int Id { get; set; }

    public int LoginId { get; set; }

    // Never store the raw refresh token.
    public string TokenHash { get; set; } = default!;

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public DateTime ExpiresAt { get; set; }

    public DateTime? RevokedAt { get; set; }

    public string? ReplacedByTokenHash { get; set; }

    public Login Login { get; set; } = default!;

    public bool IsExpired =>
        DateTime.UtcNow >= ExpiresAt;

    public bool IsRevoked =>
        RevokedAt != null;

    public bool IsActive =>
        !IsExpired && !IsRevoked;
}