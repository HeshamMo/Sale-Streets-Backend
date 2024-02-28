using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Build.Framework;
using Microsoft.EntityFrameworkCore;
using SaleStreets_Back_end.Models;
using SaleStreets_Back_end.Models.dtos;
using SaleStreets_Back_end.Services;
using SaleStreets_Back_end.Services.Products;
using SaleStreets_Back_end.Services.Tokens;

namespace SaleStreets_Back_end.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController:ControllerBase
    {
        private readonly ApplicationDbConext _context;
        private readonly IProductService _productService;
        private readonly ITokenService _tokenService;
        private readonly IImageService _imgService;
        public ProductsController(ApplicationDbConext context, IProductService productService, ITokenService tokenService, IImageService imgService)
        {
            _context = context;
            _productService = productService;
            _tokenService = tokenService;
            _imgService = imgService;
        }

        [Authorize]
        [HttpPost]

        public async Task<IActionResult> AddProduct([FromForm] ProductDto product)
        {
             IEnumerable<IFormFile> imgs = null;
         

            var form =  await this.HttpContext.Request.ReadFormAsync();

            imgs = form.Files;
     

            
            if(!ModelState.IsValid || !imgs.Any()|| imgs==null)
            {
                var y = await this.HttpContext.Request.ReadFormAsync();
               
                var x = this.Request;
                return BadRequest(new ProductModel() { Success = false, Message = "all fields must be valid" });

             }
            //string userId = await _tokenService.GetUserIdFromRequest(this.Request);
            string userId = await _tokenService.GetUserIdFromRequest(this.HttpContext);

            var productaddModel = await _productService.AddProductAsync(product, imgs, userId);

            if(productaddModel.Success)
            {
                return Ok(new { success = true , data=productaddModel.Product?.Id??0 }); 
            }

            return BadRequest(productaddModel);

        }

        [HttpGet]
        public async Task<IActionResult> SearchProducts([FromQuery] string keyword , [FromQuery] int page)
        {
            if(String.IsNullOrEmpty(keyword) | page <= 0)
            {
                return BadRequest(new { success =  false  , data = new { } , message = "Invalid Arguments"});
            }

            var products = await _productService.SearchProductsAsync(keyword, page);

            return Ok(products); 
}
        
        [HttpGet("{id}")]
        public async Task<IActionResult> GetProduct(int id)
        {
            var product = await _productService.GetProductAsync(id);
            if(product is null)
            {

                return BadRequest(new { success = false, message = "Product Not Found" });
            }

            return Ok(new {success = true , data=product , message=""});
        }

        [Authorize]
        [HttpDelete("{productId}")]
        public async Task<IActionResult> DeleteProduct(int productId)
        {
            var userId = await _tokenService.GetUserIdFromRequest(this.HttpContext);
            var result = await _productService.DeleteProductAsync(productId, userId);

            if(!result)
            {
                return BadRequest(new { success = false, message = "Deletion failed!" });
            }

            return Ok(new { success = true, message = "" }); 
        }

        [Authorize]
        [HttpPut]
        public async Task<IActionResult> UpdateProduct (int productId, ProductDto product)
        {
            var userid = await _tokenService.GetUserIdFromRequest(this.HttpContext);

            bool result = await _productService.UpdateProductAsync(productId, product, userid);
            if(!result)
            {
                return BadRequest(new { success = false, message = "Update Failed!" }); 
            }

            return Ok(new { success = true, message = "" }); 
        }

        [Authorize]
        [HttpGet("ownedProducts")]
        public async Task<IActionResult> getOwnedProducts()
        {
            var products = await _productService.getOwnedProducts(this.HttpContext);
            return Ok( products );
        }

        [HttpGet("{productId}/Images/{imgNum}")]
        public async Task<IActionResult> GetProductImage(int productId , int imgNum)
        {
            if(productId <=0 || imgNum <= 0)
            {
                return BadRequest(); 
            }
            var img = await _context.Images.Select(i => i).
                Where(i => i.product.Id == productId).Skip(imgNum - 1).FirstAsync();
                ;
            if(img is null)
            {
                return BadRequest(new { Success= false , Message="Image Not found"});
            }

            string MimeType = $"image/{img.ImgExtension}";
            
            return File(fileContents: img.ImgArr, contentType: MimeType);

        }
    }
}
