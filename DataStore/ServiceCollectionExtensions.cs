using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;

namespace DataStore
{
    public static class ServiceCollectionExtensions
    {
        public static void AddRedisCache(this IServiceCollection services)
        {
            services.AddScoped<RedisService>(); 
        }
    }
    public class RedisService(IConnectionMultiplexer connectionMultiplexer)
    {
        private readonly IDatabase userCache = connectionMultiplexer.GetDatabase(0);
        private readonly IDatabase fileCache = connectionMultiplexer.GetDatabase(1);
        private readonly IDatabase tempCache = connectionMultiplexer.GetDatabase(2);
        private readonly IDatabase campaignCache = connectionMultiplexer.GetDatabase(3);
        private readonly IDatabase siteCache = connectionMultiplexer.GetDatabase(4);
        private static readonly JsonSerializerOptions jsonSerializerOptions = new()
        {
            PropertyNameCaseInsensitive = true,
            IncludeFields = true,
        };

        public async Task SaveUserAsync<T>(string key, T value, TimeSpan expiry) where T : class
        {
            await userCache.StringSetAsync(key, JsonSerializer.Serialize(value, jsonSerializerOptions), expiry);
        }
        public async Task SaveFileAsync<T>(string key, T value, TimeSpan expiry) where T : class
        {
            await fileCache.StringSetAsync(key, JsonSerializer.Serialize(value, jsonSerializerOptions), expiry);
        }
        public async Task SaveAsync<T>(string key, T value, TimeSpan expiry) where T : class
        {
            await tempCache.StringSetAsync(key, JsonSerializer.Serialize(value, jsonSerializerOptions), expiry);
        }
        public async Task SaveCampaignAsync<T>(string key, T value, TimeSpan expiry) where T : class
        {
            if(value is List<String>)
            {
                await campaignCache.StringSetAsync($"{value}:1", JsonSerializer.Serialize(value, jsonSerializerOptions), expiry);
            }
            else
            {
                await campaignCache.StringSetAsync(key, JsonSerializer.Serialize(value, jsonSerializerOptions), expiry);
            }
        }
        public async Task SaveSitesAsync<T>(string key, T value, TimeSpan expiry) where T : class
        {
            await siteCache.StringSetAsync(key, JsonSerializer.Serialize(value, jsonSerializerOptions), expiry);
        }

        public async Task<T?> GetUserAsync<T>(string key) where T : class
        {
            var data = await userCache.StringGetAsync(key);

            if (data.IsNullOrEmpty)
                return null;

            return JsonSerializer.Deserialize<T>(data!, jsonSerializerOptions);
        }
        public async Task<T?> GetFileAsync<T>(string key) where T : class
        {
            var data = await fileCache.StringGetAsync(key);

            if (data.IsNullOrEmpty)
                return null;

            return JsonSerializer.Deserialize<T>(data!, jsonSerializerOptions);
        }
        public async Task<T?> GetAsync<T>(string key) where T : class
        {
            var data = await tempCache.StringGetAsync(key);

            if (data.IsNullOrEmpty)
                return null;

            return JsonSerializer.Deserialize<T>(data!, jsonSerializerOptions);
        }
        public async Task<T?> GetCampaignAsync<T>(string key) where T : class
        {
            var data = await campaignCache.StringGetAsync(key);

            if (data.IsNullOrEmpty)
                return null;

            return JsonSerializer.Deserialize<T>(data!, jsonSerializerOptions);
        }
        public async Task<T?> GetCampaignsAsync<T>(string key) where T : class
        {
            var data = await campaignCache.StringGetAsync($"{key}:1");

            if (data.IsNullOrEmpty)
                return null;

            return JsonSerializer.Deserialize<T>(data!, jsonSerializerOptions);
        }
        public async Task<T?> GetSitesAsync<T>(string key) where T : class
        {
            var data = await siteCache.StringGetAsync(key);

            if (data.IsNullOrEmpty)
                return null;

            return JsonSerializer.Deserialize<T>(data!, jsonSerializerOptions);
        }
    }
}
