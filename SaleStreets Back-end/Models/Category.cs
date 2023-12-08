using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SaleStreets_Back_end.Models
{
    [Table(name: "Categories")]
    public class Category
    {
        [Key]
        public int Id { get; set; }

        [MaxLength(256)]
        [Required]
        public string CategoryName { get; set; }

        [Required]
        [MaxLength(256)]
        public string Description { get; set; }

        public virtual List<Product> Products { get; set; }

    }
}