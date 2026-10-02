using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WaveApp.Core.DTOs;
using WaveApp.Core.Interfaces;

namespace WaveApp.Api.Controllers;

[ApiController]
[Route("api/products")]
public class ProductsController : ControllerBase
{
    private readonly IProductService _productService;


    public ProductsController(
        IProductService productService)
    {
        _productService =
            productService;
    }


    // =========================================================
    // GET ALL
    //
    // GET /api/products
    // =========================================================

    [AllowAnonymous]
    [HttpGet]
    public async Task<ActionResult<List<ProductReadDto>>>
        GetAll()
    {
        var products =
            await _productService
                .GetAllAsync();

        return Ok(products);
    }


    // =========================================================
    // SEARCH
    //
    // GET /api/products/search?q=headphones
    // =========================================================

    [AllowAnonymous]
    [HttpGet("search")]
    public async Task<ActionResult<List<ProductReadDto>>>
        Search(
            [FromQuery] string? q)
    {
        var products =
            await _productService
                .SearchAsync(q ?? "");

        return Ok(products);
    }


    // =========================================================
    // FEATURED
    //
    // GET /api/products/featured
    // =========================================================

    [AllowAnonymous]
    [HttpGet("featured")]
    public async Task<ActionResult<List<ProductReadDto>>>
        GetFeatured()
    {
        var products =
            await _productService
                .GetFeaturedAsync();

        return Ok(products);
    }


    // =========================================================
    // CATEGORY
    //
    // GET /api/products/category/Electronics
    // =========================================================

    [AllowAnonymous]
    [HttpGet("category/{category}")]
    public async Task<ActionResult<List<ProductReadDto>>>
        GetByCategory(
            string category)
    {
        var products =
            await _productService
                .GetByCategoryAsync(category);

        return Ok(products);
    }


    // =========================================================
    // GET BY SLUG
    //
    // GET /api/products/slug/wireless-headphones
    // =========================================================

    [AllowAnonymous]
    [HttpGet("slug/{slug}")]
    public async Task<ActionResult<ProductReadDto>>
        GetBySlug(
            string slug)
    {
        var product =
            await _productService
                .GetBySlugAsync(slug);


        if (product == null)
        {
            return NotFound(
                new
                {
                    message =
                        "Product was not found."
                });
        }


        return Ok(product);
    }


    // =========================================================
    // GET BY ID
    //
    // GET /api/products/1
    // =========================================================

    [AllowAnonymous]
    [HttpGet("{id:int}")]
    public async Task<ActionResult<ProductReadDto>>
        GetById(
            int id)
    {
        var product =
            await _productService
                .GetByIdAsync(id);


        if (product == null)
        {
            return NotFound(
                new
                {
                    message =
                        "Product was not found."
                });
        }


        return Ok(product);
    }


    // =========================================================
    // CREATE
    //
    // POST /api/products
    // =========================================================

    [Authorize]
    [HttpPost]
    public async Task<ActionResult<ProductReadDto>>
        Create(
            [FromBody] ProductCreateDto dto)
    {
        try
        {
            var product =
                await _productService
                    .CreateAsync(dto);


            return CreatedAtAction(
                nameof(GetById),
                new
                {
                    id = product.Id
                },
                product);
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(
                new
                {
                    message =
                        ex.Message
                });
        }
    }


    // =========================================================
    // UPDATE
    //
    // PUT /api/products/1
    // =========================================================

    [Authorize]
    [HttpPut("{id:int}")]
    public async Task<ActionResult<ProductReadDto>>
        Update(
            int id,
            [FromBody] ProductUpdateDto dto)
    {
        try
        {
            var product =
                await _productService
                    .UpdateAsync(
                        id,
                        dto);


            if (product == null)
            {
                return NotFound(
                    new
                    {
                        message =
                            "Product was not found."
                    });
            }


            return Ok(product);
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(
                new
                {
                    message =
                        ex.Message
                });
        }
    }


    // =========================================================
    // DELETE
    //
    // DELETE /api/products/1
    // =========================================================

    [Authorize]
    [HttpDelete("{id:int}")]
    public async Task<IActionResult>
        Delete(
            int id)
    {
        var deleted =
            await _productService
                .DeleteAsync(id);


        if (!deleted)
        {
            return NotFound(
                new
                {
                    message =
                        "Product was not found."
                });
        }


        return NoContent();
    }
}