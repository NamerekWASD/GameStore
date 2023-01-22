using System.ComponentModel.DataAnnotations;
using System.Text;

namespace ASP.NET.react.Models
{
    public class LoginModel
    {
        [Required]
        public string UserNameOrEmail { get; set; } = string.Empty;
        [Required]
        public string Password { get; set; } = string.Empty;
        [Required]
        public bool RememberMe { get; set; }

        public override string ToString()
        {
            return new StringBuilder($"First data: {UserNameOrEmail}\n")
            .Append($"Password: {Password}").ToString();
        }
    }
}