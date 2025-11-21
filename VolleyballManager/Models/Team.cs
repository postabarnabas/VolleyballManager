using System.Numerics;

namespace VolleyballManager.Models
{
    public class Team
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string City { get; set; } = string.Empty;
        public string Coach { get; set; } = string.Empty;

        public ICollection<Player> Players { get; set; } = new List<Player>();
    }
}
