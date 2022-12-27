using Microsoft.AspNetCore.Mvc;
using RedisAPI.Data;
using RedisAPI.Models;

namespace RedisAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PlatformsController : ControllerBase
    {
        private readonly IPlatformRepo _repo;

        public PlatformsController(IPlatformRepo repo)
        {
            _repo = repo;
        }

        [HttpGet]
        public ActionResult<IEnumerable<Platform>> GetPlatforms()
        {
            var platforms = _repo.GetAllPlatforms();
            return Ok(platforms);
        }

        [HttpGet("{id}", Name="GetPlatformById")]
        public ActionResult<Platform> GetPlatformById(string id)
        {
            var platform = _repo.GetPlatformById(id);
            if (platform == null)
            {
                return NotFound();
            }
            return Ok(platform);
        }

        [HttpPost]
        public ActionResult<Platform> CreatePlatform(Platform plat)
        {
            _repo.CreatePlatform(plat);
            return CreatedAtAction(nameof(GetPlatformById), new { Id = plat.Id }, plat);
        }
    }
}