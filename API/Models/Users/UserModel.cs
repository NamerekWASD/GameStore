using System.ComponentModel.DataAnnotations;

namespace API.Models.Users
{
    public class UserModel
    {
        public string ImageURL { get; set; } = string.Empty;
        public string UserName { get; set; } = string.Empty;
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Provider { get; set; } = string.Empty;
    }
}