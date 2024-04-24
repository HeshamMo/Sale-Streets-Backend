using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using SaleStreets_Back_end.Models;
using SaleStreets_Back_end.Models.dtos;
using SaleStreets_Back_end.Services.Tokens;
using static Microsoft.Extensions.Logging.EventSource.LoggingEventSource;

namespace SaleStreets_Back_end.Services.Products
{
    public class ProductService:IProductService
    {
        private readonly ApplicationDbConext _context;
        private readonly UserManager<AppUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager; 
        private readonly IImageService _imgService;
        private readonly ITokenService _tokenService;
        private readonly IMapper _mapper;
        public ProductService(ApplicationDbConext dbContext, UserManager<AppUser> userManager, IImageService imgService, RoleManager<IdentityRole> roleManager, ITokenService tokenService, IMapper mapper)
        {
            _context = dbContext;
            _userManager = userManager;
            _imgService = imgService;
            _roleManager = roleManager;
            _tokenService = tokenService;
            _mapper = mapper;
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
    
            
            


            //Product product = new Product()
            //{
            //   // Title= productDto.Title,
            //   // Description= productDto.Description,
            //    Images= images,
            //   // Location= productDto.Location,
            //   // Publisher= user ,
            //    PublishedAt = DateTime.UtcNow ,
            // //   Price = productDto.Price
            //};

            var product = _mapper.Map<Product>(productDto);
                    product.Images= images;
                    product.PublishedAt= DateTime.UtcNow;
                    product.Publisher = user;

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
                //GetProductDto product = new GetProductDto()
                //{
                //    Id = p.Id,
                //    Title = p.Title,
                //    Description = p.Description,
                //    Location = p.Location,
                //    PublishedAt = p.PublishedAt,
                //    Publisher = p.Publisher.Email,
                //    Price = p.Price,
                //    NumOfImgs = numOfImgs
                //};
                GetProductDto product = _mapper.Map<GetProductDto>(p);
                product.NumOfImgs = numOfImgs;

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
                .Select(p => new { p.Id, p.PublishedAt, p.Title, p.Description, p.Location ,p.Price })
                .Where(p => p.Title.Contains(keyword) || p.Description.Contains(keyword) )
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
                    Price = item.Price
                };

                productsDtos.Add(spd);
            }

            return productsDtos;
        }

        public async Task<IEnumerable<SearchProductsDto>> getOwnedProducts(HttpContext context ,int page)
        {
            var userId = await _tokenService.GetUserIdFromRequest(context);
            if(userId is null)
            {
                return null;
            }

            page--;
            var result = await _context.Products
                .Where(p=>p.Publisher.Id == userId)
            .Select(p => new { p.Id, p.PublishedAt, p.Title, p.Description, p.Location ,p.Price})
            .Skip(page*6)
            .Take(6)
                .OrderBy(p => p.PublishedAt)
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
                    Price = item.Price
                   
                };

                productsDtos.Add(spd);
            }

            return productsDtos;
        }

        public async Task<IEnumerable<SearchProductsDto>> SearchOwnedProducts(HttpContext context , string keyword,int page)
        {
            var userId = await _tokenService.GetUserIdFromRequest(context);
            var user = await _userManager.FindByIdAsync(userId);
            var isAdmin = await _userManager.IsInRoleAsync(user, "Admin");

                page--;
            var productsDtos = new List<SearchProductsDto>();

            if(isAdmin)
            {
                var result = await _context.Products
                    .Where(p=>(p.Title.Contains(keyword) || p.Description.Contains(keyword)))
                .Select(p => new { p.Id, p.PublishedAt, p.Title, p.Description, p.Location, p.Price })
                .Skip(page * 6)
                .Take(6)
                    .OrderBy(p => p.PublishedAt)
                  .ToListAsync();

                
                foreach(var item in result)
                {
                    productsDtos.Add( MapProductToProductDto(item.Id , item.Title , item.Description , item.Location , item.PublishedAt , item.Price));
                }
                
            }
            else
            {
                var result = await _context.Products
                    .Where(p => ((p.Title.Contains(keyword) || p.Description.Contains(keyword))&&p.Publisher.Id == userId))
                .Select(p => new { p.Id, p.PublishedAt, p.Title, p.Description, p.Location, p.Price })
                .Skip(page * 6)
                .Take(6)
                    .OrderBy(p => p.PublishedAt)
                  .ToListAsync();

                
                foreach(var item in result)
                {
                    productsDtos.Add(MapProductToProductDto(item.Id, item.Title, item.Description, item.Location, item.PublishedAt, item.Price));
                }
               

            }
            return productsDtos;
        }

        public async Task<IEnumerable<SearchProductsDto>> getLatestProducts(int page)
        {
            page--;
            var result = await _context.Products.
                OrderByDescending(p => p.PublishedAt)
                .Select(p => new { p.Id, p.PublishedAt, p.Title, p.Description, p.Location, p.Price })
                .Skip(page * 6).Take(6)
                
                .ToListAsync();

            var productsDtos = new List<SearchProductsDto>();
            foreach(var item in result)
            {
                productsDtos.Add(MapProductToProductDto(item.Id, item.Title, item.Description, item.Location, item.PublishedAt, item.Price));

            }

            return productsDtos;
        }
        public bool CheckProductPublisher(Product product, string userId)
        {
            return product.Publisher.Id== userId;
        }

        public  SearchProductsDto MapProductToProductDto(int Id , string Title , string Description ,string Location , DateTime PublishedAt, int Price)
        {
            return new SearchProductsDto()
            {
                Id = Id,
                Title = Title,
                Description = Description,
                Location = Location,
                PublishedAt = PublishedAt,
                Price = Price
            };
        }
    }
}
