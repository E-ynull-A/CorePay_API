using CorePay.Application.Interfaces.Repositories.Common;
using CorePay.Domain.Entities;
using CorePay.Domain.Utilities.Enums;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace CorePay.Persistance.Data_Access_Layer
{
    public class AppDbContextInitalizer:IAppDbContextInitalizer
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly RoleManager<IdentityRole<Guid>> _roleManager;
        private readonly IConfiguration _configuration;
        private readonly AppDbContext _context;

        public AppDbContextInitalizer(UserManager<AppUser> userManager,
                                      RoleManager<IdentityRole<Guid>> roleManager,
                                      IConfiguration configuration,
                                      AppDbContext context)
            
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _configuration = configuration;
            _context = context;
        }

        public async Task InitalizeDb()
        {
            if (await _context.Database.EnsureCreatedAsync())
                await _context.Database.MigrateAsync();
        }

        public async Task CreateSeedRolesAsync()
        {
            foreach (var role in Enum.GetValues<Role>())
            {
                if (!await _roleManager.RoleExistsAsync(role.ToString()))
                {
                    await _roleManager.CreateAsync(new IdentityRole<Guid>(role.ToString()));
                }
            }
        }


        public async Task CreateAdminInitalizer()
        {
            if ((await _userManager.GetUsersInRoleAsync(Role.Admin.ToString())).Count == 0)
            {
                AppUser admin = new AppUser(_configuration["AdminData:Name"],
                                            _configuration["AdminData:Surname"],
                                            _configuration["AdminData:Username"],
                                            DateOnly.Parse("2006-10-10"),
                                            _configuration["AdminData:Email"],
                                            _configuration["AdminData:PhoneNumber"],
                                            _configuration["AdminData:FinCode"]);

               var result = await _userManager.CreateAsync(admin, _configuration["AdminData:Password"]);

                if (!result.Succeeded)
                    Console.WriteLine(result.Errors);

                var roleResult = await _userManager.AddToRoleAsync(admin, Role.Admin.ToString());

                if(!roleResult.Succeeded)
                    Console.WriteLine(roleResult.Errors);
            }
        }
    }
}
