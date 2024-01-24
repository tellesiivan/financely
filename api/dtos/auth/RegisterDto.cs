using System.ComponentModel.DataAnnotations;

namespace api.dtos.auth;


    public class RegisterDto
    {
        [Required]
        public string Username { get; set; }= string.Empty;
        [Required]
        [EmailAddress]
        public string EmailAddress { get; set; } = string.Empty;
        [Required]
        public string Password { get; set; } =string.Empty;
        
    }