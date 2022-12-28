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
            if(plat == null)
            {
                throw new ArgumentOutOfRangeException(nameof(plat));
            }

            //var db = _redis.GetDatabase(); // get reference to default database 
            var serialPlat = JsonSerializer.Serialize(plat); // serialize the platform object

            _db.StringSet(plat.Id, serialPlat); // store the serialized platform object in the database
            // add key to set 
            _db.SetAdd("PlatformsSet", serialPlat);
        }

        public IEnumerable<Platform> GetAllPlatforms()
        {
            
            // get all platforms from set
            return _db.SetMembers("PlatformsSet").Select(p => JsonSerializer.Deserialize<Platform>(p));            
        }

        public Platform? GetPlatformById(string id)
        {
            // TODO refactor db into a centeralized location
            //var db = _redis.GetDatabase(); // get reference to default database
            var plat = _db.StringGet(id); // get the serialized platform object from the database

            // check null 
            if (!string.IsNullOrEmpty(plat))
            {
                return JsonSerializer.Deserialize<Platform>(plat); // deserialize the platform object
            }

            return null;
        }
    }
}