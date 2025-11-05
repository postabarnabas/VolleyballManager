using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace VolleyballManager.Models
{
    public class Player
    {
        public int Id { get; set; }

        [Required]
        public string FullName { get; set; } = string.Empty;

        [Required]
        public PlayerPosition Position { get; set; }

        [Required]
        public int TeamId { get; set; }
        [JsonIgnore]
        public Team? Team { get; set; }
    }
}
