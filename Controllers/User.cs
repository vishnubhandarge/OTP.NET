using System.ComponentModel.DataAnnotations;

namespace _2FAImplement.Controllers
{
    public class User
    {
        [Key]
        public int UserId { get; set; }
        public string UserName { get; set; }
        public string Name { get; set; }
        public string Password { get; set; }
        public bool TwoFactorEnabled { get; set; }
        public string? TwoFactorSecretKey { get; set; }
        public string? TwoFactorBackupCode { get; set; }
    }
}
