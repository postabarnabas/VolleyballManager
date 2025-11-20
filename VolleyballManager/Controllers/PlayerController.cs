using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization; // 🔹 szükséges
using VolleyballManager.Data;
using VolleyballManager.Models;

namespace VolleyballManager.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize] // 🔹 alapértelmezetten mindenes kell
    public class PlayersController : ControllerBase
    {
        private readonly AppDbContext _context;

        public PlayersController(AppDbContext context)
        {
            _context = context;
        }

        // 🔹 Bárki lekérheti
        [HttpGet]
        [AllowAnonymous]
        public async Task<ActionResult<IEnumerable<Player>>> GetPlayers()
        {
            return await _context.Players
                .Include(p => p.Team)
                .ToListAsync();
        }

        // 🔹 Bárki lekérheti
        [HttpGet("{id}")]
        [AllowAnonymous]
        public async Task<ActionResult<Player>> GetPlayer(int id)
        {
            var player = await _context.Players
                .Include(p => p.Team)
                .FirstOrDefaultAsync(p => p.Id == id);

            if (player == null)
                return NotFound();

            return player;
        }

        // 🔹 Csak admin hozhat létre új játékost
        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<Player>> PostPlayer(Player player)
        {
            var teamExists = await _context.Teams.AnyAsync(t => t.Id == player.TeamId);
            if (!teamExists)
                return BadRequest("A megadott csapat nem létezik.");

            _context.Players.Add(player);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetPlayer), new { id = player.Id }, player);
        }

        // 🔹 Csak admin módosíthat
        [HttpPut("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> PutPlayer(int id, Player player)
        {
            if (id != player.Id)
                return BadRequest();

            var teamExists = await _context.Teams.AnyAsync(t => t.Id == player.TeamId);
            if (!teamExists)
                return BadRequest("A megadott csapat nem létezik.");

            _context.Entry(player).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            return NoContent();
        }

        // 🔹 Csak admin törölhet
        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeletePlayer(int id)
        {
            var player = await _context.Players.FindAsync(id);
            if (player == null)
                return NotFound();

            _context.Players.Remove(player);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
