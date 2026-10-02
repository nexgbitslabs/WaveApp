using WaveApp.Core.DTOs;

namespace WaveApp.Core.Interfaces;

public interface IProductService
{
    Task<List<ProductReadDto>> GetAllAsync();

    Task<ProductReadDto?> GetByIdAsync(int id);

    Task<ProductReadDto?> GetBySlugAsync(string slug);

    Task<List<ProductReadDto>> SearchAsync(string searchTerm);

    Task<List<ProductReadDto>> GetFeaturedAsync();

    Task<List<ProductReadDto>> GetByCategoryAsync(string category);

    Task<ProductReadDto> CreateAsync(ProductCreateDto dto);

    Task<ProductReadDto?> UpdateAsync(
        int id,
        ProductUpdateDto dto);

    Task<bool> DeleteAsync(int id);
}