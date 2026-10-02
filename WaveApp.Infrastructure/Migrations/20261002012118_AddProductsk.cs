using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace WaveApp.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddProductsk : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Products",
                columns: new[] { "Id", "AllowBackorder", "Barcode", "Brand", "Category", "Color", "CompareAtPrice", "CostPrice", "CreatedAt", "Currency", "Description", "DimensionUnit", "Height", "ImageUrl", "IsActive", "IsDigital", "IsFeatured", "IsTaxable", "Length", "LowStockThreshold", "MetaDescription", "MetaTitle", "Name", "Price", "QuantityInStock", "SKU", "ShortDescription", "Size", "Slug", "SubCategory", "Tags", "TaxRate", "ThumbnailUrl", "TrackInventory", "UpdatedAt", "Weight", "WeightUnit", "Width" },
                values: new object[,]
                {
                    { 1, false, "100000000001", "Wave", "Electronics", "Black", 179.99m, 80.00m, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "CAD", "Comfortable over-ear wireless headphones designed for music, meetings, travel, and everyday use.", "cm", null, "https://placehold.co/800x600?text=Wireless+Headphones", true, false, true, true, null, 5, "Premium Wave wireless headphones with active noise cancellation.", "Wave Wireless Headphones", "Wave Wireless Headphones", 149.99m, 45, "WAVE-AUD-001", "Premium wireless headphones with active noise cancellation.", null, "wave-wireless-headphones", "Audio", "headphones,audio,wireless,bluetooth", 13.00m, "https://placehold.co/400x300?text=Wireless+Headphones", true, null, 0.35m, "kg", null },
                    { 2, false, "100000000002", "Wave", "Electronics", "Black", 139.99m, 60.00m, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "CAD", "Full-size mechanical keyboard with responsive switches and a durable professional design.", "cm", null, "https://placehold.co/800x600?text=Mechanical+Keyboard", true, false, true, true, null, 5, null, null, "Wave Mechanical Keyboard", 119.99m, 31, "WAVE-CMP-002", "Mechanical keyboard designed for developers and professionals.", null, "wave-mechanical-keyboard", "Computer Accessories", "keyboard,mechanical,computer,developer", 13.00m, "https://placehold.co/400x300?text=Mechanical+Keyboard", true, null, 0.90m, "kg", null },
                    { 3, false, null, "Wave", "Electronics", "Graphite", null, 24.00m, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "CAD", "Precision wireless mouse designed for long work sessions and everyday productivity.", "cm", null, "https://placehold.co/800x600?text=Ergonomic+Mouse", true, false, false, true, null, 10, null, null, "Wave Ergonomic Mouse", 49.99m, 72, "WAVE-CMP-003", "Comfortable wireless ergonomic mouse.", null, "wave-ergonomic-mouse", "Computer Accessories", "mouse,wireless,computer,ergonomic", 13.00m, "https://placehold.co/400x300?text=Ergonomic+Mouse", true, null, null, "kg", null },
                    { 4, false, null, "Wave", "Electronics", "Black", 499.99m, 290.00m, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "CAD", "A high-resolution 4K display suitable for software development, content creation, and office productivity.", "cm", null, "https://placehold.co/800x600?text=4K+Monitor", true, false, true, true, null, 4, null, null, "Wave 27-inch 4K Monitor", 449.99m, 18, "WAVE-DSP-004", "27-inch UHD monitor for productivity and development.", "27 inch", "wave-27-inch-4k-monitor", "Displays", "monitor,4k,display,computer", 13.00m, "https://placehold.co/400x300?text=4K+Monitor", true, null, 5.2m, "kg", null },
                    { 5, false, null, "Wave", "Electronics", "Space Grey", null, 69.00m, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "CAD", "Connect displays, network, storage, and peripherals through a single USB-C connection.", "cm", null, "https://placehold.co/800x600?text=USB-C+Dock", true, false, false, true, null, 5, null, null, "Wave USB-C Docking Station", 129.99m, 26, "WAVE-CMP-005", "Multi-port USB-C docking station for modern workstations.", null, "wave-usb-c-docking-station", "Computer Accessories", "usb-c,dock,laptop,computer", 13.00m, "https://placehold.co/400x300?text=USB-C+Dock", true, null, null, "kg", null },
                    { 6, false, null, "Wave", "Accessories", "Black", 109.99m, 42.00m, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "CAD", "Durable backpack with padded laptop storage and compartments for cables, chargers, and accessories.", "cm", null, "https://placehold.co/800x600?text=Developer+Backpack", true, false, true, true, null, 5, null, null, "Wave Developer Backpack", 89.99m, 38, "WAVE-BAG-006", "Professional laptop backpack for work and travel.", null, "wave-developer-backpack", "Bags", "backpack,laptop,travel,developer", 13.00m, "https://placehold.co/400x300?text=Developer+Backpack", true, null, null, "kg", null },
                    { 7, false, null, "Wave", "Home", "Navy", null, 12.00m, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "CAD", "Reusable insulated mug designed to keep beverages hot or cold during work and travel.", "cm", null, "https://placehold.co/800x600?text=Travel+Mug", true, false, false, true, null, 15, null, null, "Wave Insulated Travel Mug", 29.99m, 95, "WAVE-HOM-007", "Insulated stainless steel travel mug.", null, "wave-insulated-travel-mug", "Drinkware", "mug,coffee,travel,drinkware", 13.00m, "https://placehold.co/400x300?text=Travel+Mug", true, null, null, "kg", null },
                    { 8, false, null, "Wave", "Clothing", "Navy", null, 28.00m, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "CAD", "Soft everyday hoodie designed for developers, engineers, and cloud enthusiasts.", "cm", null, "https://placehold.co/800x600?text=Developer+Hoodie", true, false, true, true, null, 8, null, null, "Wave Developer Hoodie", 64.99m, 55, "WAVE-APP-008", "Comfortable Wave developer hoodie.", "L", "wave-developer-hoodie", "Hoodies", "hoodie,developer,clothing,wave", 13.00m, "https://placehold.co/400x300?text=Developer+Hoodie", true, null, null, "kg", null },
                    { 9, false, null, "Wave", "Accessories", "Silver", null, 25.00m, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "CAD", "Ergonomic laptop stand designed to improve desk posture and workstation organization.", "cm", null, "https://placehold.co/800x600?text=Laptop+Stand", true, false, false, true, null, 7, null, null, "Wave Laptop Stand", 54.99m, 42, "WAVE-CMP-009", "Adjustable aluminium laptop stand.", null, "wave-laptop-stand", "Office", "laptop,stand,office,desk", 13.00m, "https://placehold.co/400x300?text=Laptop+Stand", true, null, null, "kg", null },
                    { 10, false, null, "Wave", "Electronics", "Black", 159.99m, 85.00m, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "CAD", "Fast portable storage for source code, documents, media, backups, and development environments.", "cm", null, "https://placehold.co/800x600?text=Portable+SSD+1TB", true, false, true, true, null, 5, null, null, "Wave Portable SSD 1TB", 139.99m, 29, "WAVE-STO-010", "Compact 1TB portable solid-state drive.", null, "wave-portable-ssd-1tb", "Storage", "ssd,storage,portable,usb-c", 13.00m, "https://placehold.co/400x300?text=Portable+SSD+1TB", true, null, null, "kg", null }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 6);

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 7);

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 8);

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 9);

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 10);
        }
    }
}
