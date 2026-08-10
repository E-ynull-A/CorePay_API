using CorePay.Application.Interfaces.Services;
using CorePay.Domain.Entities;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace CorePay.Persistance.Implementations.Services
{
    public class TokenService:ITokenService
    {
        private readonly IConfiguration _configuration;

        public TokenService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public string GenerateAccessToken(AppUser appUser,
                                          string[] roles)
        {
            ICollection<Claim> claims = new List<Claim>()
            {
               new Claim(ClaimTypes.Name.ToString(),appUser.UserName),
               new Claim(ClaimTypes.GivenName.ToString(),appUser.Name),
               new Claim(ClaimTypes.Surname.ToString(),appUser.Surname),
               new Claim(ClaimTypes.NameIdentifier.ToString(),appUser.Id.ToString()),
               new Claim(ClaimTypes.Email.ToString(),appUser.Email.ToString()),
               new Claim(ClaimTypes.DateOfBirth.ToString(),appUser.Birthdate.ToShortDateString())
            };

            foreach (var item in roles)
                claims.Add(new Claim(ClaimTypes.Role.ToString(), item));

            SymmetricSecurityKey key = new SymmetricSecurityKey
                        (Encoding.ASCII.GetBytes(_configuration["JWT:SecretKey"]));

            SigningCredentials credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            double expireTime = (double.TryParse(_configuration["JWT:ExpireTime"], out double time)) ? time : 0;

            if (expireTime == 0)
                throw new Exception("Bad Configuration exception!!");


            JwtSecurityToken token = new JwtSecurityToken(issuer: _configuration["JWT:Issuer"],
                                                          audience: _configuration["JWT:Audience"],
                                                          claims: claims,
                                                          notBefore: DateTime.UtcNow,
                                                          expires: DateTime.UtcNow.AddMinutes(expireTime));

            JwtSecurityTokenHandler handler = new JwtSecurityTokenHandler();

            return handler.WriteToken(token);

        }

        public string GenerateRefreshToken()
        {
            return Guid.NewGuid().ToString();
        }
    }
}
