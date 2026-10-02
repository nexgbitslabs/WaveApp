using WaveApp.Core.DTOs;

namespace WaveApp.Core.Interfaces;

public interface IPageService
{
    Task<IEnumerable<PageInfoReadDto>> GetAllAsync();
    Task<PageInfoReadDto?> GetBySlugAsync(string slug);
}
