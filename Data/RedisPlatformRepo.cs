using System.Text.Json;
using RedisAPI.Models;
using StackExchange.Redis;

namespace RedisAPI.Data
{
    public class RedisPlatformRepo : IPlatformRepo
    {
        private readonly IConnectionMultiplexer _redis;
        private readonly IDatabase _db;

        public RedisPlatformRepo(IConnectionMultiplexer redis)
        {
            _redis = redis;
            _db = _redis.GetDatabase();
        }
        public void CreatePlatform(Platform plat)
        {
            if (plat == null)
            {
                throw new ArgumentOutOfRangeException(nameof(plat));
            }

            //var db = _redis.GetDatabase(); // get reference to default database 
            var serialPlat = JsonSerializer.Serialize(plat); // serialize the platform object

            _db.HashSet("hashplatform", new HashEntry[] {
                new HashEntry( plat.Id, serialPlat)
            });
        }

        public IEnumerable<Platform>? GetAllPlatforms()
        {

            var completHash = _db.HashGetAll("hashplatform");

            if (completHash.Length > 0)
            {
                var obj = Array.ConvertAll(completHash, x => JsonSerializer.Deserialize<Platform>(x.Value));
                return obj.ToList();
            }
            return null;

        }

        public Platform? GetPlatformById(string id)
        {
            var plat = _db.HashGet("hashplatform", id);
            if (plat.HasValue)
            {
                return JsonSerializer.Deserialize<Platform>(plat);
            }
            return null;
        }
    }
}