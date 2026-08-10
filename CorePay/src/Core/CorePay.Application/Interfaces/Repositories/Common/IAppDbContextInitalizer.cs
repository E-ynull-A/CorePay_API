using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CorePay.Application.Interfaces.Repositories.Common
{
    public interface IAppDbContextInitalizer
    {
        Task CreateSeedRolesAsync();
        Task CreateAdminInitalizer();
        Task InitalizeDb();
    }
}
