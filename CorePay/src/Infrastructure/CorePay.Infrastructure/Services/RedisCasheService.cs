using CorePay.Application.Interfaces.Services;
using Microsoft.Extensions.Caching.Distributed;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace CorePay.Infrastructure.Services
{
    public class RedisCasheService : IRedisCasheService
    {
        private readonly IDatabase _dbRedis;

        public RedisCasheService(IConnectionMultiplexer connection)
        {
            _dbRedis = connection.GetDatabase();
        }

        public async Task SetAsync<T>(string key, T value, TimeSpan expire)
        {
            var cValue = value is string str
                ? str
                : JsonSerializer.Serialize(value);

            try
            {
                await _dbRedis.StringSetAsync(key, cValue, expire);
            }
            catch (Exception ex)
            {
                throw new Exception($"Process was failed in Cashe Service! - {ex.Message}");
            }
        }

        public async Task<T?> GetAsync<T>(string key)
        {
            try
            {
                RedisValue rValue = await _dbRedis.StringGetAsync(key);

                if (!rValue.HasValue)
                    return default;

                if (typeof(T) == typeof(string))
                    return (T)(object)rValue.ToString();             

                T? value = JsonSerializer.Deserialize<T>(rValue.ToString());
                return value;
            }
            catch (Exception ex)
            {
                throw new Exception($"Process was failed in Cashe Service! - {ex.Message}");
            }
        }

        public async Task DeleteAsync(string key)
        {
            try
            {
                await _dbRedis.KeyDeleteAsync(key);
            }
            catch (Exception ex)
            {
                throw new Exception($"Process was failed in Cashe Service! - {ex.Message}");
            }
        }

        public async Task<bool> AnyAsync(string key)
        {
           return await _dbRedis.KeyExistsAsync(key);
        }
        public async Task<long> CountAsync(string key, TimeSpan expire)
        {
            if (!await _dbRedis.KeyExistsAsync(key))
            {
                await SetAsync<long>(key, 0, expire);
                return await _dbRedis.StringIncrementAsync(key);
            }
            else
                return await _dbRedis.StringIncrementAsync(key);
        }
    }
}
