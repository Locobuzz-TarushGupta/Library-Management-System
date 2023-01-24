namespace library_management_system.Models
{

    public class User
    {
        public string Username { get; set; } = string.Empty;
        public byte[] PasswordHash { get; set; }
        public byte[] PasswordSalt { get; set; }
    }

    public class ClientToken
    {
        public string Token { get; set; }
        public DateTime DateExpiration { get; set; }

    }
}
