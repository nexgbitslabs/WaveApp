namespace WaveApp.Core.Entities;

public class Login
{
    public int Id { get; set; }

    public string Username { get; set; } = default!;

    public string PasswordHash { get; set; } = default!;

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public Profile? Profile { get; set; }

    public ICollection<RefreshToken> RefreshTokens { get; set; }
        = new List<RefreshToken>();
}