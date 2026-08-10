
using CorePay.API.Middlewares;
using CorePay.Application;
using CorePay.Application.Interfaces.Repositories;
using CorePay.Domain.Entities;
using CorePay.Infrastructure;
using CorePay.Persistance;
using CorePay.Persistance.Data_Access_Layer;
using CorePay.Persistance.Implementations.Repositories;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Text.Json.Serialization;
using CorePay.Persistance;
using System.Threading.Tasks;


namespace CorePay.API
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers()
                .AddJsonOptions(opt =>
                {
                    opt.JsonSerializerOptions.Converters
                        .Add(new JsonStringEnumConverter());
                });

            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            builder.Services.RegistrateApplication();
            builder.Services.RegistrateInfrastructure(builder.Configuration);
            builder.Services.RegistratePersistance();



            builder.Services.AddDbContext<AppDbContext>(opt => opt.UseSqlServer
                        (builder.Configuration.GetConnectionString("default")));



            builder.Services.AddIdentity<AppUser, IdentityRole<Guid>>(opt =>
            {
                opt.User.RequireUniqueEmail = true;

                opt.SignIn.RequireConfirmedEmail = true;

                opt.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(3);
                opt.Lockout.MaxFailedAccessAttempts = 4;

                opt.Password.RequiredUniqueChars = 3;
                opt.Password.RequireNonAlphanumeric = false;
                opt.Password.RequiredLength = 7;
            })
                                    .AddDefaultTokenProviders()
                                    .AddEntityFrameworkStores<AppDbContext>();

           



            var app = builder.Build();

            app.UseMiddleware<GlobalExceptionMiddleware>();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            using (var scope = app.Services.CreateScope())
            {
                await app.UseDbContextInitalizer();
            }

            app.UseHttpsRedirection();

            app.UseAuthentication();
            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}
