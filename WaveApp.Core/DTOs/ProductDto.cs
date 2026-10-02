namespace WaveApp.Core.DTOs;


// =============================================================
// CREATE
// =============================================================

public class ProductCreateDto
{
    public string Name { get; set; } = default!;

    public string? Slug { get; set; }

    public string? SKU { get; set; }

    public string? Barcode { get; set; }

    public string? ShortDescription { get; set; }

    public string? Description { get; set; }

    public string? Category { get; set; }

    public string? SubCategory { get; set; }

    public string? Brand { get; set; }

    public decimal Price { get; set; }

    public decimal? CompareAtPrice { get; set; }

    public decimal? CostPrice { get; set; }

    public string Currency { get; set; } = "CAD";

    public bool IsTaxable { get; set; } = true;

    public decimal? TaxRate { get; set; }

    public bool TrackInventory { get; set; } = true;

    public int QuantityInStock { get; set; }

    public int LowStockThreshold { get; set; } = 5;

    public bool AllowBackorder { get; set; }

    public string? ImageUrl { get; set; }

    public string? ThumbnailUrl { get; set; }

    public decimal? Weight { get; set; }

    public string? WeightUnit { get; set; } = "kg";

    public decimal? Length { get; set; }

    public decimal? Width { get; set; }

    public decimal? Height { get; set; }

    public string? DimensionUnit { get; set; } = "cm";

    public string? Color { get; set; }

    public string? Size { get; set; }

    public string? Tags { get; set; }

    public bool IsActive { get; set; } = true;

    public bool IsFeatured { get; set; }

    public bool IsDigital { get; set; }

    public string? MetaTitle { get; set; }

    public string? MetaDescription { get; set; }
}


// =============================================================
// UPDATE
// =============================================================

public class ProductUpdateDto : ProductCreateDto
{
}


// =============================================================
// READ
// =============================================================

public class ProductReadDto
{
    public int Id { get; set; }

    public string Name { get; set; } = default!;

    public string Slug { get; set; } = default!;

    public string? SKU { get; set; }

    public string? Barcode { get; set; }

    public string? ShortDescription { get; set; }

    public string? Description { get; set; }

    public string? Category { get; set; }

    public string? SubCategory { get; set; }

    public string? Brand { get; set; }

    public decimal Price { get; set; }

    public decimal? CompareAtPrice { get; set; }

    public decimal? CostPrice { get; set; }

    public string Currency { get; set; } = default!;

    public bool IsTaxable { get; set; }

    public decimal? TaxRate { get; set; }

    public bool TrackInventory { get; set; }

    public int QuantityInStock { get; set; }

    public int LowStockThreshold { get; set; }

    public bool AllowBackorder { get; set; }

    public string? ImageUrl { get; set; }

    public string? ThumbnailUrl { get; set; }

    public decimal? Weight { get; set; }

    public string? WeightUnit { get; set; }

    public decimal? Length { get; set; }

    public decimal? Width { get; set; }

    public decimal? Height { get; set; }

    public string? DimensionUnit { get; set; }

    public string? Color { get; set; }

    public string? Size { get; set; }

    public string? Tags { get; set; }

    public bool IsActive { get; set; }

    public bool IsFeatured { get; set; }

    public bool IsDigital { get; set; }

    public string? MetaTitle { get; set; }

    public string? MetaDescription { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }


    // =========================================================
    // CONVENIENCE VALUES
    // =========================================================

    public bool InStock =>
        !TrackInventory ||
        QuantityInStock > 0 ||
        AllowBackorder;


    public bool IsOnSale =>
        CompareAtPrice.HasValue &&
        CompareAtPrice.Value > Price;
}