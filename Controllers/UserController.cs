using _2FAImplement.Data;
using _2FAImplement.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using OtpNet;

namespace _2FAImplement.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly UserDbContext _userDbContext;

        public UserController(UserDbContext userDbContext)
        {
            _userDbContext = userDbContext;
        }
        [HttpPost]
        public async Task<IActionResult> Login(LoginDto login)
        {
            var user = await _userDbContext.Users.FindAsync(login.Id);
            if (user == null)
            {
                return NotFound();
            }

            else if(user.Password != login.Password)
            {
                return BadRequest("Wrong Password");
            }
            return Ok("Enter 2FA code in Login2FA method generated from phone.");
        }
        [HttpPost]
        public async Task<IActionResult> Register(RegisterDto register)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest("Invalid data");
            }

            var secretKey = Generate2FASecretKey();

            if(secretKey == null)
            {
                return BadRequest("2fa security key generation failed.");
            }

            User user = new User()
            {
                UserName = register.UserName,
                Name = register.Name,
                Password = register.Password,
                TwoFactorEnabled = true,
                TwoFactorSecretKey = secretKey,
                TwoFactorBackupCode = null 
            };

            var result = await _userDbContext.Users.AddAsync(user);
                
            await _userDbContext.SaveChangesAsync();

            return Ok($"User Created. secret key is {secretKey}.");        
        }

        [HttpPost]
        public async Task<IActionResult> Verify2FA(int id , string otp)
        {
            var user = await _userDbContext.Users.FindAsync(id);

            if (user == null)
            {
                return NotFound("User not found");
            }
            var totp = new Totp(Base32Encoding.ToBytes(user.TwoFactorSecretKey));
            if (!totp.VerifyTotp(otp, out _))
            {
                return BadRequest("Invalid 2FA code");
            }

            var token = new Guid();

            return Ok(new { Token = token });
        }
        private string Generate2FASecretKey()
        {
            var bytes = KeyGeneration.GenerateRandomKey(20);
            return Base32Encoding.ToString(bytes);
        }
    }
}
