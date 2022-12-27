using System.ComponentModel.DataAnnotations;

namespace RedisAPI.Models
{
    public class Platform 
    {
        [Required]
        public string Id { get; set; } = $"platfirn:{Guid.NewGuid().ToString()}";

        [Required]
        public string Name { get; set; } = String.Empty;
    }
}