using System.ComponentModel.DataAnnotations;

namespace Movie_Collection.Authentication.DTO
{
    public class RegisterUserDTO
    {
        [EmailAddress]
        public string Email { get; set; } = string.Empty;
        public string Username { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
    }
}
