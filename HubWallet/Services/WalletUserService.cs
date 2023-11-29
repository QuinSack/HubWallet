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

        public WalletUserService(UserManager<IdentityUser> userManager, SignInManager<IdentityUser> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
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
            var keys = Encoding.ASCII.GetBytes("djdjks-djk2393-djksd-338932jds-vbkwjje-39393");
            var issuer = "HUBTELWALLET";
            var audience = "JWTServicePostmanClient";

            //var claims = new List<Claim>
            //{
            //    new Claim(ClaimTypes.Name, user.UserName),
            //    new Claim(ClaimTypes.NameIdentifier, user.Id),
            //    // Add additional claims as needed
            //};

            //var securityTokenDescriptor = new SecurityTokenDescriptor
            //{
            //    Subject = new ClaimsIdentity(claims),
            //    Expires = DateTime.UtcNow.AddHours(1), // Token expiration time
            //    SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature),
            //    Issuer = issuer,
            //    Audience = audience,
            //};

            //var tokenHandler = new JwtSecurityTokenHandler();
            //var token = tokenHandler.CreateToken(securityTokenDescriptor);

            //return tokenHandler.WriteToken(token);
            List<Claim> claims = new List<Claim>()
            {
                new Claim(ClaimTypes.Name, user.UserName),
                new Claim(ClaimTypes.NameIdentifier, user.Id),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("djdjks-djk2393-djksd-338932jds-vbkwjje-39393"));

            var cred = new SigningCredentials(key, SecurityAlgorithms.HmacSha512);

            var token = new JwtSecurityToken(
                 issuer,
                audience,
                claims: claims,
                //expires: DateTime.UtcNow.AddSeconds(double.Parse(_config.GetSection("JwtConfig:Expires").Value!)),
                signingCredentials: cred
                );

            var jwt = new JwtSecurityTokenHandler().WriteToken(token);
            return jwt;
        }
    }
}
