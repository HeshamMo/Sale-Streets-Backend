using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.InteropServices;
using Microsoft.EntityFrameworkCore;
using System.Text.Json.Serialization;
using SaleStreets_Back_end.Models;

namespace SaleStreets_Back_end.Models
{
    [Table(name:"Products")]
    [Index(nameof(Title))]
    public class Product
    {
        [Key]       
        public int Id { get; set; }

        [Required]
        [StringLength(256,ErrorMessage = "Title Length Can Not Exceed 256 Characters!" , MinimumLength =2)]
        public string Title { get; set; }

        [Required]
        [MaxLength(512)]
        public string Description { get; set; }

        [Required]
        public DateTime PublishedAt { get; set; }

        [Required]
        [MaxLength(256)]
        public string Location { get; set; }

        [Required]
        public int Price { get; set; }

        [Required]
        [JsonIgnore]
        public virtual AppUser Publisher { get; set; }



        [Required]
        public virtual List<Image> Images { get; set; }
       

    }

}
