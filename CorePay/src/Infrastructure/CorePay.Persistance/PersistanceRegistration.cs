using CorePay.Persistance.Data_Access_Layer;
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

            return services;
        }
    }
}
