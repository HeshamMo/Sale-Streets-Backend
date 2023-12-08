using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using SaleStreets_Back_end.Services;
using Newtonsoft.Json;

namespace SaleStreets_Back_end.Models
{
    [Table(name:"Images")]

    public class Image
    {
        private readonly IImageService _imageService;

        [Key]
        public int Id { get; set; }

        public Product product { get; set; }

        [Required]
        [Column(name:"Images")]
        [JsonIgnore]
        public byte[] ImgArr { get; set; }

        [Required]
        [JsonIgnore]
        public string ImgExtension { get; set; }

        [NotMapped]
        public string ImgString { 
            get {
                string imgString = $"data:Image/{ImgExtension};base64:" +
                $"{Convert.ToBase64String(ImgArr)}";

                return imgString;
            
            } 
        }

        public Image(IImageService imageService , IFormFile image)
        {
            _imageService = imageService;
            this.ImgArr = _imageService.ConvertFormImageToByteArray(image);
            this.ImgExtension = _imageService.GetImgExtension(image);
        }

        public Image(IImageService imageService)
        {
            _imageService = imageService;
        }

        public Image() { }

    }
}
