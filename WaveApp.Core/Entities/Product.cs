using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WaveApp.Core.Entities;

public class Product
{
    // =========================================================
    // IDENTITY
    // =========================================================

    public int Id { get; set; }


    [Required]
    [MaxLength(200)]
    public string Name { get; set; } = default!;


    [Required]
    [MaxLength(200)]
    public string Slug { get; set; } = default!;


    [MaxLength(100)]
    public string? SKU { get; set; }


    [MaxLength(100)]
    public string? Barcode { get; set; }


    // =========================================================
    // DESCRIPTION
    // =========================================================

    [MaxLength(500)]
    public string? ShortDescription { get; set; }


    public string? Description { get; set; }


    // =========================================================
    // CATEGORY / BRAND
    // =========================================================

    [MaxLength(150)]
    public string? Category { get; set; }


    [MaxLength(150)]
    public string? SubCategory { get; set; }


    [MaxLength(150)]
    public string? Brand { get; set; }


    // =========================================================
    // PRICING
    // =========================================================

    [Column(TypeName = "decimal(18,2)")]
    public decimal Price { get; set; }


    [Column(TypeName = "decimal(18,2)")]
    public decimal? CompareAtPrice { get; set; }


    [Column(TypeName = "decimal(18,2)")]
    public decimal? CostPrice { get; set; }


    [MaxLength(10)]
    public string Currency { get; set; } = "CAD";


    // =========================================================
    // TAX
    // =========================================================

    public bool IsTaxable { get; set; } = true;


    [Column(TypeName = "decimal(5,2)")]
    public decimal? TaxRate { get; set; }


    // =========================================================
    // INVENTORY
    // =========================================================

    public bool TrackInventory { get; set; } = true;


    public int QuantityInStock { get; set; }


    public int LowStockThreshold { get; set; } = 5;


    public bool AllowBackorder { get; set; } = false;


    // =========================================================
    // PRODUCT IMAGE
    // =========================================================

    [MaxLength(1000)]
    public string? ImageUrl { get; set; }


    [MaxLength(1000)]
    public string? ThumbnailUrl { get; set; }


    // =========================================================
    // PHYSICAL PRODUCT INFORMATION
    // =========================================================

    [Column(TypeName = "decimal(10,2)")]
    public decimal? Weight { get; set; }


    [MaxLength(20)]
    public string? WeightUnit { get; set; } = "kg";


    [Column(TypeName = "decimal(10,2)")]
    public decimal? Length { get; set; }


    [Column(TypeName = "decimal(10,2)")]
    public decimal? Width { get; set; }


    [Column(TypeName = "decimal(10,2)")]
    public decimal? Height { get; set; }


    [MaxLength(20)]
    public string? DimensionUnit { get; set; } = "cm";


    // =========================================================
    // PRODUCT ATTRIBUTES
    // =========================================================

    [MaxLength(100)]
    public string? Color { get; set; }


    [MaxLength(100)]
    public string? Size { get; set; }


    [MaxLength(500)]
    public string? Tags { get; set; }


    // =========================================================
    // PRODUCT STATE
    // =========================================================

    public bool IsActive { get; set; } = true;


    public bool IsFeatured { get; set; } = false;


    public bool IsDigital { get; set; } = false;


    // =========================================================
    // SEO
    // =========================================================

    [MaxLength(200)]
    public string? MetaTitle { get; set; }


    [MaxLength(500)]
    public string? MetaDescription { get; set; }


    // =========================================================
    // AUDIT
    // =========================================================

    public DateTime CreatedAt { get; set; } =
        DateTime.UtcNow;


    public DateTime? UpdatedAt { get; set; }
}