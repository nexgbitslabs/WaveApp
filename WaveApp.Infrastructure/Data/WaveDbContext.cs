using Microsoft.EntityFrameworkCore;
using WaveApp.Core.Entities;

namespace WaveApp.Infrastructure.Data;

public class WaveDbContext : DbContext
{
    public WaveDbContext(DbContextOptions<WaveDbContext> options) : base(options) { }

    public DbSet<Login> Logins => Set<Login>();
    public DbSet<Profile> Profiles => Set<Profile>();
    public DbSet<PageInfo> Pages => Set<PageInfo>();
    public DbSet<RefreshToken> RefreshTokens =>
    Set<RefreshToken>();
    public DbSet<Product> Products =>
    Set<Product>();

    protected override void OnModelCreating(
        ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Login>()
            .HasOne(x => x.Profile)
            .WithOne(x => x.Login)
            .HasForeignKey<Profile>(x => x.LoginId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<RefreshToken>()
            .HasOne(x => x.Login)
            .WithMany(x => x.RefreshTokens)
            .HasForeignKey(x => x.LoginId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<RefreshToken>()
            .HasIndex(x => x.TokenHash)
            .IsUnique();

        modelBuilder.Entity<Product>(
            entity =>
            {
                entity.HasKey(x => x.Id);


                entity.HasIndex(x => x.Slug)
                    .IsUnique();


                entity.HasIndex(x => x.SKU)
                    .IsUnique();


                entity.Property(x => x.Price)
                    .HasPrecision(18, 2);


                entity.Property(x => x.CompareAtPrice)
                    .HasPrecision(18, 2);


                entity.Property(x => x.CostPrice)
                    .HasPrecision(18, 2);


                entity.Property(x => x.TaxRate)
                    .HasPrecision(5, 2);
            }
        );

    modelBuilder.Entity<Product>()
    .HasData(

        // =====================================================
        // PRODUCT 1
        // =====================================================

        new Product
        {
            Id = 1,

            Name =
                "Wave Wireless Headphones",

            Slug =
                "wave-wireless-headphones",

            SKU =
                "WAVE-AUD-001",

            Barcode =
                "100000000001",

            ShortDescription =
                "Premium wireless headphones with active noise cancellation.",

            Description =
                "Comfortable over-ear wireless headphones designed for music, meetings, travel, and everyday use.",

            Category =
                "Electronics",

            SubCategory =
                "Audio",

            Brand =
                "Wave",

            Price =
                149.99m,

            CompareAtPrice =
                179.99m,

            CostPrice =
                80.00m,

            Currency =
                "CAD",

            IsTaxable =
                true,

            TaxRate =
                13.00m,

            TrackInventory =
                true,

            QuantityInStock =
                45,

            LowStockThreshold =
                5,

            AllowBackorder =
                false,

            ImageUrl =
                "https://placehold.co/800x600?text=Wireless+Headphones",

            ThumbnailUrl =
                "https://placehold.co/400x300?text=Wireless+Headphones",

            Weight =
                0.35m,

            WeightUnit =
                "kg",

            Color =
                "Black",

            Tags =
                "headphones,audio,wireless,bluetooth",

            IsActive =
                true,

            IsFeatured =
                true,

            IsDigital =
                false,

            MetaTitle =
                "Wave Wireless Headphones",

            MetaDescription =
                "Premium Wave wireless headphones with active noise cancellation.",

            CreatedAt =
                new DateTime(
                    2026,
                    1,
                    1,
                    0,
                    0,
                    0,
                    DateTimeKind.Utc)
        },


        // =====================================================
        // PRODUCT 2
        // =====================================================

        new Product
        {
            Id = 2,

            Name =
                "Wave Mechanical Keyboard",

            Slug =
                "wave-mechanical-keyboard",

            SKU =
                "WAVE-CMP-002",

            Barcode =
                "100000000002",

            ShortDescription =
                "Mechanical keyboard designed for developers and professionals.",

            Description =
                "Full-size mechanical keyboard with responsive switches and a durable professional design.",

            Category =
                "Electronics",

            SubCategory =
                "Computer Accessories",

            Brand =
                "Wave",

            Price =
                119.99m,

            CompareAtPrice =
                139.99m,

            CostPrice =
                60.00m,

            Currency =
                "CAD",

            IsTaxable =
                true,

            TaxRate =
                13.00m,

            TrackInventory =
                true,

            QuantityInStock =
                31,

            LowStockThreshold =
                5,

            ImageUrl =
                "https://placehold.co/800x600?text=Mechanical+Keyboard",

            ThumbnailUrl =
                "https://placehold.co/400x300?text=Mechanical+Keyboard",

            Weight =
                0.90m,

            WeightUnit =
                "kg",

            Color =
                "Black",

            Tags =
                "keyboard,mechanical,computer,developer",

            IsActive =
                true,

            IsFeatured =
                true,

            CreatedAt =
                new DateTime(
                    2026,
                    1,
                    1,
                    0,
                    0,
                    0,
                    DateTimeKind.Utc)
        },


        // =====================================================
        // PRODUCT 3
        // =====================================================

        new Product
        {
            Id = 3,

            Name =
                "Wave Ergonomic Mouse",

            Slug =
                "wave-ergonomic-mouse",

            SKU =
                "WAVE-CMP-003",

            ShortDescription =
                "Comfortable wireless ergonomic mouse.",

            Description =
                "Precision wireless mouse designed for long work sessions and everyday productivity.",

            Category =
                "Electronics",

            SubCategory =
                "Computer Accessories",

            Brand =
                "Wave",

            Price =
                49.99m,

            CostPrice =
                24.00m,

            Currency =
                "CAD",

            IsTaxable =
                true,

            TaxRate =
                13.00m,

            TrackInventory =
                true,

            QuantityInStock =
                72,

            LowStockThreshold =
                10,

            ImageUrl =
                "https://placehold.co/800x600?text=Ergonomic+Mouse",

            ThumbnailUrl =
                "https://placehold.co/400x300?text=Ergonomic+Mouse",

            Color =
                "Graphite",

            Tags =
                "mouse,wireless,computer,ergonomic",

            IsActive =
                true,

            IsFeatured =
                false,

            CreatedAt =
                new DateTime(
                    2026,
                    1,
                    1,
                    0,
                    0,
                    0,
                    DateTimeKind.Utc)
        },


        // =====================================================
        // PRODUCT 4
        // =====================================================

        new Product
        {
            Id = 4,

            Name =
                "Wave 27-inch 4K Monitor",

            Slug =
                "wave-27-inch-4k-monitor",

            SKU =
                "WAVE-DSP-004",

            ShortDescription =
                "27-inch UHD monitor for productivity and development.",

            Description =
                "A high-resolution 4K display suitable for software development, content creation, and office productivity.",

            Category =
                "Electronics",

            SubCategory =
                "Displays",

            Brand =
                "Wave",

            Price =
                449.99m,

            CompareAtPrice =
                499.99m,

            CostPrice =
                290.00m,

            Currency =
                "CAD",

            IsTaxable =
                true,

            TaxRate =
                13.00m,

            TrackInventory =
                true,

            QuantityInStock =
                18,

            LowStockThreshold =
                4,

            ImageUrl =
                "https://placehold.co/800x600?text=4K+Monitor",

            ThumbnailUrl =
                "https://placehold.co/400x300?text=4K+Monitor",

            Weight =
                5.2m,

            WeightUnit =
                "kg",

            Color =
                "Black",

            Size =
                "27 inch",

            Tags =
                "monitor,4k,display,computer",

            IsActive =
                true,

            IsFeatured =
                true,

            CreatedAt =
                new DateTime(
                    2026,
                    1,
                    1,
                    0,
                    0,
                    0,
                    DateTimeKind.Utc)
        },


        // =====================================================
        // PRODUCT 5
        // =====================================================

        new Product
        {
            Id = 5,

            Name =
                "Wave USB-C Docking Station",

            Slug =
                "wave-usb-c-docking-station",

            SKU =
                "WAVE-CMP-005",

            ShortDescription =
                "Multi-port USB-C docking station for modern workstations.",

            Description =
                "Connect displays, network, storage, and peripherals through a single USB-C connection.",

            Category =
                "Electronics",

            SubCategory =
                "Computer Accessories",

            Brand =
                "Wave",

            Price =
                129.99m,

            CostPrice =
                69.00m,

            Currency =
                "CAD",

            IsTaxable =
                true,

            TaxRate =
                13.00m,

            TrackInventory =
                true,

            QuantityInStock =
                26,

            LowStockThreshold =
                5,

            ImageUrl =
                "https://placehold.co/800x600?text=USB-C+Dock",

            ThumbnailUrl =
                "https://placehold.co/400x300?text=USB-C+Dock",

            Color =
                "Space Grey",

            Tags =
                "usb-c,dock,laptop,computer",

            IsActive =
                true,

            IsFeatured =
                false,

            CreatedAt =
                new DateTime(
                    2026,
                    1,
                    1,
                    0,
                    0,
                    0,
                    DateTimeKind.Utc)
        },


        // =====================================================
        // PRODUCT 6
        // =====================================================

        new Product
        {
            Id = 6,

            Name =
                "Wave Developer Backpack",

            Slug =
                "wave-developer-backpack",

            SKU =
                "WAVE-BAG-006",

            ShortDescription =
                "Professional laptop backpack for work and travel.",

            Description =
                "Durable backpack with padded laptop storage and compartments for cables, chargers, and accessories.",

            Category =
                "Accessories",

            SubCategory =
                "Bags",

            Brand =
                "Wave",

            Price =
                89.99m,

            CompareAtPrice =
                109.99m,

            CostPrice =
                42.00m,

            Currency =
                "CAD",

            IsTaxable =
                true,

            TaxRate =
                13.00m,

            TrackInventory =
                true,

            QuantityInStock =
                38,

            LowStockThreshold =
                5,

            ImageUrl =
                "https://placehold.co/800x600?text=Developer+Backpack",

            ThumbnailUrl =
                "https://placehold.co/400x300?text=Developer+Backpack",

            Color =
                "Black",

            Tags =
                "backpack,laptop,travel,developer",

            IsActive =
                true,

            IsFeatured =
                true,

            CreatedAt =
                new DateTime(
                    2026,
                    1,
                    1,
                    0,
                    0,
                    0,
                    DateTimeKind.Utc)
        },


        // =====================================================
        // PRODUCT 7
        // =====================================================

        new Product
        {
            Id = 7,

            Name =
                "Wave Insulated Travel Mug",

            Slug =
                "wave-insulated-travel-mug",

            SKU =
                "WAVE-HOM-007",

            ShortDescription =
                "Insulated stainless steel travel mug.",

            Description =
                "Reusable insulated mug designed to keep beverages hot or cold during work and travel.",

            Category =
                "Home",

            SubCategory =
                "Drinkware",

            Brand =
                "Wave",

            Price =
                29.99m,

            CostPrice =
                12.00m,

            Currency =
                "CAD",

            IsTaxable =
                true,

            TaxRate =
                13.00m,

            TrackInventory =
                true,

            QuantityInStock =
                95,

            LowStockThreshold =
                15,

            ImageUrl =
                "https://placehold.co/800x600?text=Travel+Mug",

            ThumbnailUrl =
                "https://placehold.co/400x300?text=Travel+Mug",

            Color =
                "Navy",

            Tags =
                "mug,coffee,travel,drinkware",

            IsActive =
                true,

            IsFeatured =
                false,

            CreatedAt =
                new DateTime(
                    2026,
                    1,
                    1,
                    0,
                    0,
                    0,
                    DateTimeKind.Utc)
        },


        // =====================================================
        // PRODUCT 8
        // =====================================================

        new Product
        {
            Id = 8,

            Name =
                "Wave Developer Hoodie",

            Slug =
                "wave-developer-hoodie",

            SKU =
                "WAVE-APP-008",

            ShortDescription =
                "Comfortable Wave developer hoodie.",

            Description =
                "Soft everyday hoodie designed for developers, engineers, and cloud enthusiasts.",

            Category =
                "Clothing",

            SubCategory =
                "Hoodies",

            Brand =
                "Wave",

            Price =
                64.99m,

            CostPrice =
                28.00m,

            Currency =
                "CAD",

            IsTaxable =
                true,

            TaxRate =
                13.00m,

            TrackInventory =
                true,

            QuantityInStock =
                55,

            LowStockThreshold =
                8,

            ImageUrl =
                "https://placehold.co/800x600?text=Developer+Hoodie",

            ThumbnailUrl =
                "https://placehold.co/400x300?text=Developer+Hoodie",

            Color =
                "Navy",

            Size =
                "L",

            Tags =
                "hoodie,developer,clothing,wave",

            IsActive =
                true,

            IsFeatured =
                true,

            CreatedAt =
                new DateTime(
                    2026,
                    1,
                    1,
                    0,
                    0,
                    0,
                    DateTimeKind.Utc)
        },


        // =====================================================
        // PRODUCT 9
        // =====================================================

        new Product
        {
            Id = 9,

            Name =
                "Wave Laptop Stand",

            Slug =
                "wave-laptop-stand",

            SKU =
                "WAVE-CMP-009",

            ShortDescription =
                "Adjustable aluminium laptop stand.",

            Description =
                "Ergonomic laptop stand designed to improve desk posture and workstation organization.",

            Category =
                "Accessories",

            SubCategory =
                "Office",

            Brand =
                "Wave",

            Price =
                54.99m,

            CostPrice =
                25.00m,

            Currency =
                "CAD",

            IsTaxable =
                true,

            TaxRate =
                13.00m,

            TrackInventory =
                true,

            QuantityInStock =
                42,

            LowStockThreshold =
                7,

            ImageUrl =
                "https://placehold.co/800x600?text=Laptop+Stand",

            ThumbnailUrl =
                "https://placehold.co/400x300?text=Laptop+Stand",

            Color =
                "Silver",

            Tags =
                "laptop,stand,office,desk",

            IsActive =
                true,

            IsFeatured =
                false,

            CreatedAt =
                new DateTime(
                    2026,
                    1,
                    1,
                    0,
                    0,
                    0,
                    DateTimeKind.Utc)
        },


        // =====================================================
        // PRODUCT 10
        // =====================================================

        new Product
        {
            Id = 10,

            Name =
                "Wave Portable SSD 1TB",

            Slug =
                "wave-portable-ssd-1tb",

            SKU =
                "WAVE-STO-010",

            ShortDescription =
                "Compact 1TB portable solid-state drive.",

            Description =
                "Fast portable storage for source code, documents, media, backups, and development environments.",

            Category =
                "Electronics",

            SubCategory =
                "Storage",

            Brand =
                "Wave",

            Price =
                139.99m,

            CompareAtPrice =
                159.99m,

            CostPrice =
                85.00m,

            Currency =
                "CAD",

            IsTaxable =
                true,

            TaxRate =
                13.00m,

            TrackInventory =
                true,

            QuantityInStock =
                29,

            LowStockThreshold =
                5,

            ImageUrl =
                "https://placehold.co/800x600?text=Portable+SSD+1TB",

            ThumbnailUrl =
                "https://placehold.co/400x300?text=Portable+SSD+1TB",

            Color =
                "Black",

            Tags =
                "ssd,storage,portable,usb-c",

            IsActive =
                true,

            IsFeatured =
                true,

            CreatedAt =
                new DateTime(
                    2026,
                    1,
                    1,
                    0,
                    0,
                    0,
                    DateTimeKind.Utc)
        }
    );
    }

    

    
}
