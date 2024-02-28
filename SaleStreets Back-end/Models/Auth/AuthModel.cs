using Newtonsoft.Json;
using System.Text.Json.Serialization;

namespace SaleStreets_Back_end.Models.Auth
{
    public class AuthModel
    {
        public bool Success { get; set; }
        public string Email { get; set; } 
        public List<string> Roles { get; set; }
        [System.Text.Json.Serialization.JsonIgnore]    
        public string Token { get; set; } 
        public string Message { get; set; }
        

    }
}


