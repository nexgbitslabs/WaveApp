namespace WaveApp.Core.DTOs;

public class AuthResponseDto
{
    public string AccessToken { get; set; } = default!;

    public string RefreshToken { get; set; } = default!;

    public DateTime AccessTokenExpiresAt { get; set; }

    public DateTime RefreshTokenExpiresAt { get; set; }

    public int LoginId { get; set; }

    public string Username { get; set; } = default!;

    public string FullName { get; set; } = default!;

    public string Email { get; set; } = default!;

    public string Role { get; set; } = default!;
}