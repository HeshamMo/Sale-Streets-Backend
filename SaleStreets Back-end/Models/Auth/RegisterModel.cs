using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace SaleStreets_Back_end.Models.Auth
{
    public class RegisterModel
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
        public string PhoneNumber { get; set; }

        [EmailAddress]
        public string Email { get; set; }

    }
}
