using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Microsoft.IdentityModel.Tokens.Experimental;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace CorePay.Application
{
    public static class ApplicationRegistration
    {
        public static IServiceCollection RegistrateServices(this IServiceCollection services,IConfiguration configuration)
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




            services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly
                                    (Assembly.GetExecutingAssembly()));

            return services;
        }
    }
}
