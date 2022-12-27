using System.Text.Json;
using RedisAPI.Models;
using StackExchange.Redis;

namespace RedisAPI.Data
{
    public class RedisPlatformRepo : IPlatformRepo
    {
        private readonly IConnectionMultiplexer _redis;

        public RedisPlatformRepo(IConnectionMultiplexer redis)
        {
            _redis = redis;
        }
        public void CreatePlatform(Platform plat)
        {
            if(plat == null)
            {
                throw new ArgumentOutOfRangeException(nameof(plat));
            }

            var db = _redis.GetDatabase(); // get reference to default database 
            var serialPlat = JsonSerializer.Serialize(plat); // serialize the platform object

            db.StringSet(plat.Id, serialPlat); // store the serialized platform object in the database
        }

        public IEnumerable<Platform> GetAllPlatforms()
        {
            throw new NotImplementedException();
        }

        public Platform? GetPlatformById(string id)
        {
            // TODO refactor db into a centeralized location
            var db = _redis.GetDatabase(); // get reference to default database
            var plat = db.StringGet(id); // get the serialized platform object from the database

            // check null 
            if (!string.IsNullOrEmpty(plat))
            {
                return JsonSerializer.Deserialize<Platform>(plat); // deserialize the platform object
            }

            return null;
        }
    }
}