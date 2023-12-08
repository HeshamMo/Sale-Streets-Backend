using SaleStreets_Back_end.Migrations;
using SaleStreets_Back_end.Models;

namespace SaleStreets_Back_end.Services
{
    public interface IImageService
    {
        public byte[] ConvertFormImageToByteArray(IFormFile image);
        public string ConvertByteArrayToImageString(byte[] ImageArr , string ImgExtension);
        public string GetImgExtension(IFormFile image);

        public List<Image> FormsImgsToListOfImgs(IEnumerable<IFormFile> imgs);
        public List<string> ImgsToListOfImgStrings(IEnumerable<Image> imgs);
    }
}
