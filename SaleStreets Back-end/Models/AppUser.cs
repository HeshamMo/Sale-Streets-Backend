using Microsoft.AspNetCore.Identity;
using SaleStreetProject.CustomValidators;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace SaleStreets_Back_end.Models
{
    public class AppUser : IdentityUser
    {
        [Required]
        [StringLength(256, MinimumLength = 2)]
        public string FirstName { get; set; }


        [Required]
        [StringLength(256, MinimumLength = 2)]
        public string LastName { get; set; }

        [Required]
        [StringLength(256, MinimumLength = 2)]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [AllowNull]
        [DataType(dataType: DataType.PhoneNumber)]
        public override string PhoneNumber { get; set; }


        public virtual List<Product> Products { get; set; }
    }
}
