using CorePay.Application.Interfaces.Repositories;
using CorePay.Application.Interfaces.Repositories.Common;
using CorePay.Persistance.Data_Access_Layer;
using CorePay.Persistance.Implementations.Repositories;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CorePay.Persistance
{
    public static class PersistanceRegistration
    {

        public static IServiceCollection RegistratePersistance(this IServiceCollection services)
        {
            services.AddScoped<IAppDbContextInitalizer, AppDbContextInitalizer>();
            services.AddScoped<IAccountRepository, AccountRepository>();
            services.AddScoped<ICardRepository, CardRepository>();
            services.AddScoped<ITransactionRepository, TransactionRepository>();
            services.AddScoped<IRefreshTokenRepository, RefreshTokenRepository>();
            services.AddScoped<ITransferRepository, TransferRepository>();
            services.AddScoped<IUnitOfWork, UnitOfWork>();


            return services;
        }
        public static async Task<IApplicationBuilder> UseDbContextInitalizer(this IApplicationBuilder app)
        {
            using (var scope = app.ApplicationServices.CreateAsyncScope())
            {
                var initalizer = scope.ServiceProvider.GetRequiredService<IAppDbContextInitalizer>();

               await initalizer.CreateSeedRolesAsync();
               await initalizer.CreateAdminInitalizer();
            }

            return app;
        }
    }
}
