using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace SaleStreets_Back_end.Models.Auth
{
    public class LogInModel
    {
        [Required]
        [StringLength(256, MinimumLength = 2)]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [EmailAddress]
        public string Email { get; set; }
    }
}
