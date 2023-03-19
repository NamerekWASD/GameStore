namespace API.Models.Users
{
    public class ConfirmationModel
    {
        public string Email { get; set; } = string.Empty;
        public string Code { get; set; } = string.Empty;
        public bool RememberMe { get; set; } = false;
    }
}