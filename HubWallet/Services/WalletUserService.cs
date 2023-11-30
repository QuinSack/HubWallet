using HubWallet.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace HubWallet.Services
{
    public class WalletUserService
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly IConfiguration _config;


        public WalletUserService(UserManager<IdentityUser> userManager, SignInManager<IdentityUser> signInManager, IConfiguration config)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _config = config;
        }

        public async Task<bool> RegisterUser(WalletUser walletUser)
        {
            var user = new IdentityUser { UserName = walletUser.PhoneNumber, PhoneNumber = walletUser.PhoneNumber };
            var result = await _userManager.CreateAsync(user, walletUser.Password);

            if (!result.Succeeded)
            {
                foreach (var error in result.Errors)
                {
                    Console.WriteLine($"Error: {error.Description}");
                }
            }

            return result.Succeeded;
        }

        public async Task<string> Login(WalletUser walletUser)
        {
            var user = await _userManager.FindByNameAsync(walletUser.PhoneNumber);
            if (user != null)
            {
                var result = await _signInManager.CheckPasswordSignInAsync(user, walletUser.Password, false);
                if (result.Succeeded)
                {
                    return GenerateJwtToken(user);
                }
            }

            return null;
        }

        private string GenerateJwtToken(IdentityUser user)
        {
            List<Claim> claims = new List<Claim>()
            {
                new Claim(ClaimTypes.Name, user.UserName),
                new Claim(ClaimTypes.NameIdentifier, user.Id)
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(
                _config.GetSection("JwtConfig:Secret").Value!));

            var cred = new SigningCredentials(key, SecurityAlgorithms.HmacSha512);

            var token = new JwtSecurityToken(
                 _config["JwtConfig:Issuer"],
                _config["JwtConfig:Audience"],
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(60),
                signingCredentials: cred
                );

            var jwt = new JwtSecurityTokenHandler().WriteToken(token);
            return jwt;
        }
    }
}
