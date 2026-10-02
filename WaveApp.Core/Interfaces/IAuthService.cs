using WaveApp.Core.DTOs;

namespace WaveApp.Core.Interfaces;

public interface IAuthService
{
    Task<AuthResponseDto?> LoginAsync(
        string username,
        string password);

    Task<AuthResponseDto?> RefreshAsync(
        string refreshToken);

    Task<bool> LogoutAsync(
        string refreshToken);

    Task<ProfileReadDto?> GetProfileAsync(
        string username);

    Task<RegisterReadDto> RegisterAsync(
        RegisterDto dto);
}