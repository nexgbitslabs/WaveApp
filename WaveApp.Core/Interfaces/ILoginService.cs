using WaveApp.Core.DTOs;

namespace WaveApp.Application.Interfaces;

public interface ILoginService
{
    Task<IReadOnlyList<LoginReadDto>> GetAllAsync();

    Task<LoginReadDto?> GetByIdAsync(int id);

    Task<LoginReadDto> CreateAsync(
        LoginCreateDto dto);

    Task<LoginReadDto?> UpdateAsync(
        int id,
        LoginUpdateDto dto);

    Task<bool> DeleteAsync(int id);
}