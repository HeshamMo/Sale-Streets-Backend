using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using SaleStreets_Back_end.Models;
using SaleStreets_Back_end.Models.dtos;

namespace SaleStreets_Back_end.Services.Products
{
    public class ProductService:IProductService
    {
        private readonly ApplicationDbConext _context;
        private readonly UserManager<AppUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager; 
        private readonly IImageService _imgService;

        public ProductService(ApplicationDbConext dbContext, UserManager<AppUser> userManager, IImageService imgService, RoleManager<IdentityRole> roleManager)
        {
            _context = dbContext;
            _userManager = userManager;
            _imgService = imgService;
            _roleManager = roleManager;
        }

        public async Task<ProductModel> AddProductAsync(
            ProductDto productDto, IEnumerable<IFormFile> imgs ,string UserId)
        {
            var user = await _userManager.FindByIdAsync(UserId);
            if (user == null)
            {
                return new ProductModel() { Success= false , Message="An Error Occured" };
            }

            var images =  _imgService.FormsImgsToListOfImgs(imgs);
    
            
            


            Product product = new Product()
            {
                Title= productDto.Title,
                Description= productDto.Description,
                Images= images,
                Location= productDto.Location,
                Publisher= user ,
                PublishedAt = DateTime.UtcNow
            };

            try
            {
                await _context.Products.AddAsync(product);
                await _context.SaveChangesAsync();

                return new ProductModel()
                { Success = true, Product = product, Message = null }; 
            }
            catch (Exception ex)
            {

                return new ProductModel() 
                { Success = false, Message = "An Error Occured!" }; 
            }

        }



        public async Task<bool> DeleteProductAsync(int id , string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if(user == null) return false;

            var roles = await _userManager.GetRolesAsync(user);

            var product = await _context.Products.Include(p => p.Publisher).FirstOrDefaultAsync(p => p.Id == id);
            if(product is null) return false;

            if(CheckProductPublisher(product , userId)|| roles.Contains("admin"))
            {
                try
                {
                    _context.Products.Remove(product);
                    await _context.SaveChangesAsync();
                    return true;
                }
                catch(Exception ex)
                {
                    return false;
                }

            }
            else
            {
                return false;
            }

           
            
        }




        public async Task<GetProductDto> GetProductAsync(int id)
        {
            var p = await _context.Products.Include(p=>p.Publisher).FirstOrDefaultAsync(p=>p.Id== id);

            if (p is not null)
            {
            int numOfImgs = await _context.Images.CountAsync(img=>img.product.Id == p.Id);
            GetProductDto product = new GetProductDto()

            {
                Id = p.Id,
                Title = p.Title,
                Description = p.Description,
                Location = p.Location,
                PublishedAt = p.PublishedAt,
                Publisher = p.Publisher.Email,
                NumOfImgs = numOfImgs
            };

                return product; 
            }

            
            return null;
        }


        public async Task<bool> UpdateProductAsync(int id, ProductDto updatedProduct , string userId)
        {
            var product =  await _context.Products.FirstOrDefaultAsync(p=>p.Id== id);
            if (product is null || !CheckProductPublisher(product, userId) )
            {
                return false; 
            }


            product.Location = updatedProduct.Location;
            product.Title = updatedProduct.Title;
            product.Description = updatedProduct.Description;

            try
            {
                _context.Products.Update(product);
                await _context.SaveChangesAsync(); 
                return true;
            }
            catch (Exception ex)
            {
                return false; 
            }
            
        }

        public async Task<IEnumerable<SearchProductsDto>> SearchProductsAsync(string keyword, int page)
        {
            page--;
            var result = await _context.Products
                .Select(p => new { p.Id, p.PublishedAt, p.Title, p.Description, p.Location })
                .Skip(page * 6).Take(6)
                .ToListAsync();
            
            var productsDtos = new List<SearchProductsDto>();
            foreach(var item in result)
            {
                SearchProductsDto spd = new SearchProductsDto()
                {
                    Id = item.Id,
                    Title = item.Title,
                    Description = item.Description,
                    Location = item.Location,
                    PublishedAt = item.PublishedAt,
                };

                productsDtos.Add(spd);
            }

            return productsDtos;
        }

        public bool CheckProductPublisher(Product product, string userId)
        {
            return product.Publisher.Id== userId;
        }
    }
}
