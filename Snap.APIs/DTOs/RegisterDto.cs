using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Snap.APIs.DTOs
{
    public class RegisterDto
    {

        [Required]
        [EmailAddress]
        public string Email { get; set; }
        [Required]
        public string DispalyName { get; set; }

        [Required]
        [RegularExpression(
            @"^(?=.*[A-Z])(?=.*\d)(?=.*[_\-\W]).{6,20}$",
            ErrorMessage = "Password must be 6-20 characters, with at least 1 uppercase letter, 1 number, and 1 special character (e.g., _, -, @, $, etc.)."
        )]
        [JsonPropertyName("password")] 
        public string password { get; set; }



    }
}
