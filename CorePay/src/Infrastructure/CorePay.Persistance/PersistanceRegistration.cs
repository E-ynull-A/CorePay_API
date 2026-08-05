using CorePay.Application.Interfaces.Repositories;
using CorePay.Persistance.Data_Access_Layer;
using CorePay.Persistance.Implementations.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace CorePay.Persistance
{
    public static class PersistanceRegistration
    {
        public static IServiceCollection RegistrateServices(this IServiceCollection services,IConfiguration config)
        {
            services.AddDbContext<AppDbContext>(opt => opt.UseSqlServer
                                    (config.GetConnectionString("default")));

            services.AddScoped<IAccountRepository, AccountRepository>();
            services.AddScoped<ICardRepository, CardRepository>();
            services.AddScoped<ITransactionRepository, TransactionRepository>();

            return services;
        }
    }
}
