namespace SneakerOnlineShop.DTO
{
    public class ProductDTO
    {
        public int ProductId { get; set; }
        public string ProductName { get; set; } = null!;
        public int? CategoryId { get; set; }
        public string? QuantityPerUnit { get; set; }
        public decimal? UnitPrice { get; set; }
        public short? UnitsInStock { get; set; }
        public short? UnitsOnOrder { get; set; }
        public short? Size { get; set; }
        public string? Color { get; set; }
        public string? Description { get; set; }
        public string? PresentImage { get; set; }

    }
}
