namespace TaskManager_api.Models
{
    public class AuthResult
    {
        public string Username { get; set; }
        public string Role { get; set; }
        public string Token { get; set; }
        public string? Logo { get; set; }
    }
}
