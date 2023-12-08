using System.ComponentModel.DataAnnotations;

namespace SaleStreets_Back_end.Models.dtos
{
    public class ProductDto
    {
        [Required]
        [StringLength(256, ErrorMessage = "Title Length Can Not Exceed 256 Characters!", MinimumLength = 2)]
        public string Title { get; set; }

        [Required]
        [MaxLength(512)]
        public string Description { get; set; }


        [Required]
        [MaxLength(256)]
        public string Location { get; set; }
        

    }

}
