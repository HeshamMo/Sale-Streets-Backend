using System.IO;
using SaleStreets_Back_end.Models;
using SaleStreets_Back_end.Services;

namespace SaleStreets_Back_end.Services
{
    public class ImageService:IImageService
    {
        public string ConvertByteArrayToImageString(byte[] ImageArr , string ImgExtension)
        {
            string imgString = $"data:Image/{ImgExtension};base64:" +
                $"{Convert.ToBase64String(ImageArr)}";

            return imgString ;
          
        }



        public byte[] ConvertFormImageToByteArray(IFormFile image)
        {

            using(var memoryStream = new MemoryStream())
            {
                image.CopyTo(memoryStream);
                return memoryStream.ToArray();
            }

        }

        public List<Image> FormsImgsToListOfImgs(IEnumerable<IFormFile> formImgs)
        {
            List<Image> imgsList = new List<Image>();

            foreach(var imgsItem in formImgs)
            {
                imgsList.Add(
                    new Image(this, imgsItem) 
                    );
            }

            return imgsList;

        }

        public string GetImgExtension(IFormFile image)
        {
            string extension = System.IO.Path.GetExtension(image.FileName);

            return extension;
        }

        public  List<string> ImgsToListOfImgStrings(IEnumerable<Image> imgs)
        {
            List<string> imgsStrings = new List<string>();
            
            foreach(Image img in imgs) {
                imgsStrings.Add(this.ConvertByteArrayToImageString(img.ImgArr, img.ImgExtension));
            }
            
            return imgsStrings;
        }
    }
}
