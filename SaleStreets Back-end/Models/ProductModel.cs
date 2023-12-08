using SaleStreets_Back_end.Models.dtos;

namespace SaleStreets_Back_end.Models
{
    public class ProductModel
    {
        public bool Success { get; set; }

        public Product? Product { get; set; }
        public string? Message { get; set; }
    }
}
