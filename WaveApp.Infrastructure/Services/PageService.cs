using Microsoft.EntityFrameworkCore;
using WaveApp.Core.DTOs;
using WaveApp.Core.Interfaces;
using WaveApp.Infrastructure.Data;

namespace WaveApp.Infrastructure.Services;

public class PageService : IPageService
{
    private readonly WaveDbContext _db;

    public PageService(WaveDbContext db) => _db = db;

    public async Task<IEnumerable<PageInfoReadDto>> GetAllAsync()
        => await _db.Pages
            .Select(p => new PageInfoReadDto(p.Id, p.Slug, p.Title, p.Content))
            .ToListAsync();

    public async Task<PageInfoReadDto?> GetBySlugAsync(string slug)
        => await _db.Pages
            .Where(p => p.Slug == slug)
            .Select(p => new PageInfoReadDto(p.Id, p.Slug, p.Title, p.Content))
            .FirstOrDefaultAsync();
}
