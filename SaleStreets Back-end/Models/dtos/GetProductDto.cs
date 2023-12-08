using System.ComponentModel.DataAnnotations;

namespace SaleStreets_Back_end.Models.dtos
{
    public class GetProductDto
    {
        public int Id { get; set; }


        public string Title { get; set; }


        public string Description { get; set; }


        public DateTime PublishedAt { get; set; }

    
        public string Location { get; set; }

   
     
        public  string Publisher { get; set; }


        public int NumOfImgs { get; set; }


    }
}
