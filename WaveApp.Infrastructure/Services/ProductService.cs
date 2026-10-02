using Microsoft.EntityFrameworkCore;
using WaveApp.Core.DTOs;
using WaveApp.Core.Entities;
using WaveApp.Core.Interfaces;
using WaveApp.Infrastructure.Data;

namespace WaveApp.Infrastructure.Services;

public class ProductService : IProductService
{
    private readonly WaveDbContext _db;

    public ProductService(
        WaveDbContext db)
    {
        _db = db;
    }


    // =========================================================
    // GET ALL
    // =========================================================

    public async Task<List<ProductReadDto>> GetAllAsync()
    {
        return await _db.Products
            .AsNoTracking()
            .Where(x => x.IsActive)
            .OrderByDescending(x => x.CreatedAt)
            .Select(x => MapToReadDto(x))
            .ToListAsync();
    }


    // =========================================================
    // GET BY ID
    // =========================================================

    public async Task<ProductReadDto?> GetByIdAsync(
        int id)
    {
        var product =
            await _db.Products
                .AsNoTracking()
                .FirstOrDefaultAsync(
                    x => x.Id == id);

        return product == null
            ? null
            : MapToReadDto(product);
    }


    // =========================================================
    // GET BY SLUG
    // =========================================================

    public async Task<ProductReadDto?> GetBySlugAsync(
        string slug)
    {
        var normalizedSlug =
            slug.Trim().ToLowerInvariant();


        var product =
            await _db.Products
                .AsNoTracking()
                .FirstOrDefaultAsync(
                    x =>
                        x.Slug == normalizedSlug &&
                        x.IsActive);

        return product == null
            ? null
            : MapToReadDto(product);
    }


    // =========================================================
    // SEARCH
    // =========================================================

    public async Task<List<ProductReadDto>> SearchAsync(
        string searchTerm)
    {
        if (string.IsNullOrWhiteSpace(searchTerm))
        {
            return await GetAllAsync();
        }


        var term =
            searchTerm.Trim().ToLower();


        return await _db.Products
            .AsNoTracking()
            .Where(x =>
                x.IsActive &&
                (
                    x.Name.ToLower().Contains(term) ||

                    (
                        x.SKU != null &&
                        x.SKU.ToLower().Contains(term)
                    ) ||

                    (
                        x.Brand != null &&
                        x.Brand.ToLower().Contains(term)
                    ) ||

                    (
                        x.Category != null &&
                        x.Category.ToLower().Contains(term)
                    ) ||

                    (
                        x.SubCategory != null &&
                        x.SubCategory.ToLower().Contains(term)
                    ) ||

                    (
                        x.ShortDescription != null &&
                        x.ShortDescription
                            .ToLower()
                            .Contains(term)
                    ) ||

                    (
                        x.Tags != null &&
                        x.Tags
                            .ToLower()
                            .Contains(term)
                    )
                )
            )
            .OrderBy(x => x.Name)
            .Select(x => MapToReadDto(x))
            .ToListAsync();
    }


    // =========================================================
    // FEATURED
    // =========================================================

    public async Task<List<ProductReadDto>>
        GetFeaturedAsync()
    {
        return await _db.Products
            .AsNoTracking()
            .Where(x =>
                x.IsActive &&
                x.IsFeatured)
            .OrderByDescending(x => x.CreatedAt)
            .Select(x => MapToReadDto(x))
            .ToListAsync();
    }


    // =========================================================
    // CATEGORY
    // =========================================================

    public async Task<List<ProductReadDto>>
        GetByCategoryAsync(
            string category)
    {
        if (string.IsNullOrWhiteSpace(category))
        {
            return new List<ProductReadDto>();
        }


        var value =
            category.Trim().ToLower();


        return await _db.Products
            .AsNoTracking()
            .Where(x =>
                x.IsActive &&
                x.Category != null &&
                x.Category.ToLower() == value)
            .OrderBy(x => x.Name)
            .Select(x => MapToReadDto(x))
            .ToListAsync();
    }


    // =========================================================
    // CREATE
    // =========================================================

    public async Task<ProductReadDto> CreateAsync(
        ProductCreateDto dto)
    {
        if (string.IsNullOrWhiteSpace(dto.Name))
        {
            throw new InvalidOperationException(
                "Product name is required.");
        }


        if (dto.Price < 0)
        {
            throw new InvalidOperationException(
                "Product price cannot be negative.");
        }


        var slug =
            string.IsNullOrWhiteSpace(dto.Slug)
                ? GenerateSlug(dto.Name)
                : GenerateSlug(dto.Slug);


        slug =
            await GenerateUniqueSlugAsync(
                slug);


        if (!string.IsNullOrWhiteSpace(dto.SKU))
        {
            var sku =
                dto.SKU.Trim();


            var skuExists =
                await _db.Products
                    .AnyAsync(x =>
                        x.SKU == sku);


            if (skuExists)
            {
                throw new InvalidOperationException(
                    $"A product with SKU '{sku}' already exists.");
            }
        }


        var product =
            new Product
            {
                Name =
                    dto.Name.Trim(),

                Slug =
                    slug,

                SKU =
                    Normalize(dto.SKU),

                Barcode =
                    Normalize(dto.Barcode),

                ShortDescription =
                    Normalize(dto.ShortDescription),

                Description =
                    Normalize(dto.Description),

                Category =
                    Normalize(dto.Category),

                SubCategory =
                    Normalize(dto.SubCategory),

                Brand =
                    Normalize(dto.Brand),

                Price =
                    dto.Price,

                CompareAtPrice =
                    dto.CompareAtPrice,

                CostPrice =
                    dto.CostPrice,

                Currency =
                    string.IsNullOrWhiteSpace(
                        dto.Currency)
                        ? "CAD"
                        : dto.Currency
                            .Trim()
                            .ToUpperInvariant(),

                IsTaxable =
                    dto.IsTaxable,

                TaxRate =
                    dto.TaxRate,

                TrackInventory =
                    dto.TrackInventory,

                QuantityInStock =
                    dto.QuantityInStock,

                LowStockThreshold =
                    dto.LowStockThreshold,

                AllowBackorder =
                    dto.AllowBackorder,

                ImageUrl =
                    Normalize(dto.ImageUrl),

                ThumbnailUrl =
                    Normalize(dto.ThumbnailUrl),

                Weight =
                    dto.Weight,

                WeightUnit =
                    Normalize(dto.WeightUnit),

                Length =
                    dto.Length,

                Width =
                    dto.Width,

                Height =
                    dto.Height,

                DimensionUnit =
                    Normalize(dto.DimensionUnit),

                Color =
                    Normalize(dto.Color),

                Size =
                    Normalize(dto.Size),

                Tags =
                    Normalize(dto.Tags),

                IsActive =
                    dto.IsActive,

                IsFeatured =
                    dto.IsFeatured,

                IsDigital =
                    dto.IsDigital,

                MetaTitle =
                    Normalize(dto.MetaTitle),

                MetaDescription =
                    Normalize(dto.MetaDescription),

                CreatedAt =
                    DateTime.UtcNow
            };


        _db.Products.Add(product);

        await _db.SaveChangesAsync();


        return MapToReadDto(product);
    }


    // =========================================================
    // UPDATE
    // =========================================================

    public async Task<ProductReadDto?> UpdateAsync(
        int id,
        ProductUpdateDto dto)
    {
        var product =
            await _db.Products
                .FirstOrDefaultAsync(
                    x => x.Id == id);


        if (product == null)
        {
            return null;
        }


        if (string.IsNullOrWhiteSpace(dto.Name))
        {
            throw new InvalidOperationException(
                "Product name is required.");
        }


        if (dto.Price < 0)
        {
            throw new InvalidOperationException(
                "Product price cannot be negative.");
        }


        // -----------------------------------------------------
        // SKU uniqueness
        // -----------------------------------------------------

        if (!string.IsNullOrWhiteSpace(dto.SKU))
        {
            var sku =
                dto.SKU.Trim();


            var skuExists =
                await _db.Products
                    .AnyAsync(x =>
                        x.Id != id &&
                        x.SKU == sku);


            if (skuExists)
            {
                throw new InvalidOperationException(
                    $"A product with SKU '{sku}' already exists.");
            }
        }


        // -----------------------------------------------------
        // SLUG
        // -----------------------------------------------------

        var requestedSlug =
            string.IsNullOrWhiteSpace(dto.Slug)
                ? GenerateSlug(dto.Name)
                : GenerateSlug(dto.Slug);


        if (!string.Equals(
            requestedSlug,
            product.Slug,
            StringComparison.OrdinalIgnoreCase))
        {
            requestedSlug =
                await GenerateUniqueSlugAsync(
                    requestedSlug,
                    id);
        }


        // -----------------------------------------------------
        // UPDATE PRODUCT
        // -----------------------------------------------------

        product.Name =
            dto.Name.Trim();

        product.Slug =
            requestedSlug;

        product.SKU =
            Normalize(dto.SKU);

        product.Barcode =
            Normalize(dto.Barcode);

        product.ShortDescription =
            Normalize(dto.ShortDescription);

        product.Description =
            Normalize(dto.Description);

        product.Category =
            Normalize(dto.Category);

        product.SubCategory =
            Normalize(dto.SubCategory);

        product.Brand =
            Normalize(dto.Brand);

        product.Price =
            dto.Price;

        product.CompareAtPrice =
            dto.CompareAtPrice;

        product.CostPrice =
            dto.CostPrice;

        product.Currency =
            string.IsNullOrWhiteSpace(
                dto.Currency)
                ? "CAD"
                : dto.Currency
                    .Trim()
                    .ToUpperInvariant();

        product.IsTaxable =
            dto.IsTaxable;

        product.TaxRate =
            dto.TaxRate;

        product.TrackInventory =
            dto.TrackInventory;

        product.QuantityInStock =
            dto.QuantityInStock;

        product.LowStockThreshold =
            dto.LowStockThreshold;

        product.AllowBackorder =
            dto.AllowBackorder;

        product.ImageUrl =
            Normalize(dto.ImageUrl);

        product.ThumbnailUrl =
            Normalize(dto.ThumbnailUrl);

        product.Weight =
            dto.Weight;

        product.WeightUnit =
            Normalize(dto.WeightUnit);

        product.Length =
            dto.Length;

        product.Width =
            dto.Width;

        product.Height =
            dto.Height;

        product.DimensionUnit =
            Normalize(dto.DimensionUnit);

        product.Color =
            Normalize(dto.Color);

        product.Size =
            Normalize(dto.Size);

        product.Tags =
            Normalize(dto.Tags);

        product.IsActive =
            dto.IsActive;

        product.IsFeatured =
            dto.IsFeatured;

        product.IsDigital =
            dto.IsDigital;

        product.MetaTitle =
            Normalize(dto.MetaTitle);

        product.MetaDescription =
            Normalize(dto.MetaDescription);

        product.UpdatedAt =
            DateTime.UtcNow;


        await _db.SaveChangesAsync();


        return MapToReadDto(product);
    }


    // =========================================================
    // DELETE
    // =========================================================

    public async Task<bool> DeleteAsync(
        int id)
    {
        var product =
            await _db.Products
                .FirstOrDefaultAsync(
                    x => x.Id == id);


        if (product == null)
        {
            return false;
        }


        _db.Products.Remove(product);

        await _db.SaveChangesAsync();


        return true;
    }


    // =========================================================
    // MAP
    // =========================================================

    private static ProductReadDto MapToReadDto(
        Product product)
    {
        return new ProductReadDto
        {
            Id =
                product.Id,

            Name =
                product.Name,

            Slug =
                product.Slug,

            SKU =
                product.SKU,

            Barcode =
                product.Barcode,

            ShortDescription =
                product.ShortDescription,

            Description =
                product.Description,

            Category =
                product.Category,

            SubCategory =
                product.SubCategory,

            Brand =
                product.Brand,

            Price =
                product.Price,

            CompareAtPrice =
                product.CompareAtPrice,

            CostPrice =
                product.CostPrice,

            Currency =
                product.Currency,

            IsTaxable =
                product.IsTaxable,

            TaxRate =
                product.TaxRate,

            TrackInventory =
                product.TrackInventory,

            QuantityInStock =
                product.QuantityInStock,

            LowStockThreshold =
                product.LowStockThreshold,

            AllowBackorder =
                product.AllowBackorder,

            ImageUrl =
                product.ImageUrl,

            ThumbnailUrl =
                product.ThumbnailUrl,

            Weight =
                product.Weight,

            WeightUnit =
                product.WeightUnit,

            Length =
                product.Length,

            Width =
                product.Width,

            Height =
                product.Height,

            DimensionUnit =
                product.DimensionUnit,

            Color =
                product.Color,

            Size =
                product.Size,

            Tags =
                product.Tags,

            IsActive =
                product.IsActive,

            IsFeatured =
                product.IsFeatured,

            IsDigital =
                product.IsDigital,

            MetaTitle =
                product.MetaTitle,

            MetaDescription =
                product.MetaDescription,

            CreatedAt =
                product.CreatedAt,

            UpdatedAt =
                product.UpdatedAt
        };
    }


    // =========================================================
    // GENERATE SLUG
    // =========================================================

    private static string GenerateSlug(
        string value)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            return Guid.NewGuid()
                .ToString("N");
        }


        var slug =
            value
                .Trim()
                .ToLowerInvariant();


        var characters =
            slug.Select(c =>
                char.IsLetterOrDigit(c)
                    ? c
                    : '-')
                .ToArray();


        slug =
            new string(characters);


        while (slug.Contains("--"))
        {
            slug =
                slug.Replace("--", "-");
        }


        return slug.Trim('-');
    }


    // =========================================================
    // UNIQUE SLUG
    // =========================================================

    private async Task<string>
        GenerateUniqueSlugAsync(
            string slug,
            int? excludeProductId = null)
    {
        var baseSlug =
            slug;

        var candidate =
            baseSlug;

        var number =
            1;


        while (
            await _db.Products.AnyAsync(x =>
                x.Slug == candidate &&
                (
                    excludeProductId == null ||
                    x.Id != excludeProductId
                )))
        {
            candidate =
                $"{baseSlug}-{number}";

            number++;
        }


        return candidate;
    }


    // =========================================================
    // NORMALIZE OPTIONAL STRING
    // =========================================================

    private static string? Normalize(
        string? value)
    {
        return string.IsNullOrWhiteSpace(value)
            ? null
            : value.Trim();
    }
}