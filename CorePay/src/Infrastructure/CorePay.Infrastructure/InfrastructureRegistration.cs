using CorePay.Application.Interfaces.Services;
using CorePay.Domain.Entities;
using CorePay.Persistance.Implementations.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace CorePay.Infrastructure
{
    public static class InfrastructureRegistration
    {
        private const int MINUTE = 5;
        public static IServiceCollection RegistrateInfrastructure(this IServiceCollection services
                                                    ,IConfiguration configuration)
        {
            services.AddAuthentication(opt =>
            {
                opt.DefaultAuthenticateScheme = default;
                opt.DefaultChallengeScheme = default;
            })
           .AddJwtBearer(opt => opt.TokenValidationParameters = new()
           {
               ValidateAudience = true,
               ValidateIssuer = true,
               ValidateIssuerSigningKey = true,
               ValidateLifetime = true,

               ValidAudience = configuration["JWT:Audience"],
               ValidIssuer = configuration["JWT:Issuer"],
               IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(configuration["JWT:SecurityKey"])),
               LifetimeValidator = (_, exp, token, _) => exp is not null && token is not null ? exp > DateTime.UtcNow : false,

               ClockSkew = TimeSpan.Zero
           });

            

            services.AddScoped<ITokenService, TokenService>();

            return services;
        }
    }
}
