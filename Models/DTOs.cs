namespace _2FAImplement.Models
{
    public class DTOs
    {
    }
    public class RegisterDto
    {
        public string UserName { get; set; } = null!;
        public string Name { get; set; } = null!;
        public string Password { get; set; } = null!;
    }

    public class LoginDto
    {
        public int Id  { get; set; }
        public string Password { get; set; }
    }
}
