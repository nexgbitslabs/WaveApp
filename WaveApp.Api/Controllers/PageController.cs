using Microsoft.AspNetCore.Mvc;
using WaveApp.Core.Interfaces;

namespace WaveApp.Api.Controllers;

[ApiController]
[Route("api/pages")]
public class PageController : ControllerBase
{
    private readonly IPageService _pages;

    public PageController(IPageService pages) => _pages = pages;

    [HttpGet]
    public async Task<IActionResult> GetAll()
        => Ok(await _pages.GetAllAsync());

    [HttpGet("{slug}")]
    public async Task<IActionResult> GetBySlug(string slug)
    {
        var page = await _pages.GetBySlugAsync(slug);
        return page is null ? NotFound() : Ok(page);
    }
}
