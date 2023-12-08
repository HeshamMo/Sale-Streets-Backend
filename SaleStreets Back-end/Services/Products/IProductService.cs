using SaleStreets_Back_end.Models;
using SaleStreets_Back_end.Models.dtos;

namespace SaleStreets_Back_end.Services.Products
{
    public interface IProductService
    {
        public  Task<GetProductDto> GetProductAsync(int id);

        public Task<bool> UpdateProductAsync (int id , ProductDto product, string userId);

        public Task<ProductModel> AddProductAsync(ProductDto productDto, IEnumerable<IFormFile> imgs, string UserId);

        public Task<bool> DeleteProductAsync(int id , string userId);

        public Task<IEnumerable<SearchProductsDto>> SearchProductsAsync(string keyword  , int page);

        public bool CheckProductPublisher(Product product, string userId);
    }
}

