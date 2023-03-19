using System.ComponentModel.DataAnnotations;
using System.Text;

namespace API.Models.Users
{
    public class LoginModel
    {
        [Required]
        public string Email { get; set; } = string.Empty;
    }
}