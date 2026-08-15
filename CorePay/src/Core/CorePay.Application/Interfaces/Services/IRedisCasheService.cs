using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CorePay.Application.Interfaces.Services
{
    public interface IRedisCasheService
    {
        Task SetAsync<T>(string key, T value, TimeSpan expire);
        Task<T?> GetAsync<T>(string key);
        Task DeleteAsync(string key);
        Task<long> CountAsync(string key, TimeSpan expire);
    }
}
