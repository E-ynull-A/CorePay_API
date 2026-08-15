using CorePay.Application.Behaviors;
using CorePay.Domain.Entities;
using FluentValidation;
using MediatR;
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
        public static IServiceCollection RegistrateApplication(this IServiceCollection services)
        {         

            services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly
                                    (Assembly.GetExecutingAssembly()));

            services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());
            services.AddScoped(typeof(IPipelineBehavior<,>),
                               typeof(ValidationBehavior<,>));

            services.AddAutoMapper(Assembly.GetExecutingAssembly());
   


            return services;
        }
    }
}
