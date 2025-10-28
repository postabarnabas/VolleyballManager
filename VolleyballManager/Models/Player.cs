using System.ComponentModel.DataAnnotations;

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

        public Team? Team { get; set; }
    }
}
