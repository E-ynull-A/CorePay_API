using CorePay.Domain.Entities;
using MediatR;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace CorePay.Application.Features.Queries.Token.Access
{
    public class PostAccessTokenQueryHandler : IRequestHandler<PostAccessTokenQuery, PostAccessTokenQueryResponse>
    {
        private readonly IConfiguration _configuration;

        public PostAccessTokenQueryHandler(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        public async Task<PostAccessTokenQueryResponse> Handle(PostAccessTokenQuery request, CancellationToken cancellationToken)
        {
            AppUser appUser = request.User;

            ICollection<Claim> claims = new List<Claim>()
            {
               new Claim(ClaimTypes.Name.ToString(),appUser.UserName),
               new Claim(ClaimTypes.GivenName.ToString(),appUser.Name),
               new Claim(ClaimTypes.Surname.ToString(),appUser.Surname),
               new Claim(ClaimTypes.NameIdentifier.ToString(),appUser.Id.ToString()),
               new Claim(ClaimTypes.Email.ToString(),appUser.Email.ToString()),
               new Claim(ClaimTypes.DateOfBirth.ToString(),appUser.Birthdate.ToShortDateString())
            };

            foreach (var item in request.Roles)
               claims.Add(new Claim(ClaimTypes.Role.ToString(), item));

            SymmetricSecurityKey key = new SymmetricSecurityKey
                        (Encoding.ASCII.GetBytes(_configuration["JWT:SecretKey"]));

            SigningCredentials credentials = new SigningCredentials(key,SecurityAlgorithms.HmacSha256);

            JwtSecurityToken token = new JwtSecurityToken(issuer:_configuration["JWT:Issuer"],
                                                          audience: _configuration["JWT:Audience"],
                                                          claims:claims,
                                                          notBefore:DateTime.UtcNow,
                                                          expires:DateTime.UtcNow.AddMinutes(request.Minutes));

            JwtSecurityTokenHandler handler = new JwtSecurityTokenHandler();

            return new PostAccessTokenQueryResponse
                        (appUser.UserName,token.ValidTo,handler.WriteToken(token));

        }
    }
}
