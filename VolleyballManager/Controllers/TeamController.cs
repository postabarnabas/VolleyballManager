using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using VolleyballManager.Data;
using VolleyballManager.Models;

namespace VolleyballManager.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize] // 🔹 EZ HIÁNYZOTT!
    public class TeamsController : ControllerBase
    {
        private readonly AppDbContext _context;

        public TeamsController(AppDbContext context)
        {
            _context = context;
        }

        // GET: api/teams - bárki láthatja
        [HttpGet]
        [AllowAnonymous]
        public async Task<ActionResult<IEnumerable<Team>>> GetTeams()
        {
            return await _context.Teams
                .Include(t => t.Players)
                .ToListAsync();
        }

        // GET: api/teams/5 - bárki láthatja
        [HttpGet("{id}")]
        [AllowAnonymous]
        public async Task<ActionResult<Team>> GetTeam(int id)
        {
            var team = await _context.Teams
                .Include(t => t.Players)
                .FirstOrDefaultAsync(t => t.Id == id);

            if (team == null)
                return NotFound();

            return team;
        }

        // POST: api/teams - CSAK ADMIN
        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<Team>> PostTeam(Team team)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            _context.Teams.Add(team);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetTeam), new { id = team.Id }, team);
        }

        // PUT: api/teams/5 - CSAK ADMIN
        [HttpPut("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> PutTeam(int id, Team team)
        {
            if (id != team.Id)
                return BadRequest("A megadott ID nem egyezik a team objektum ID-jával.");

            if (!TeamExists(id))
                return NotFound("A csapat nem található.");

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            _context.Entry(team).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TeamExists(id))
                    return NotFound();
                throw;
            }

            return NoContent();
        }

        // DELETE: api/teams/5 - CSAK ADMIN
        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteTeam(int id)
        {
            var team = await _context.Teams.FindAsync(id);
            if (team == null)
                return NotFound();

            _context.Teams.Remove(team);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool TeamExists(int id)
        {
            return _context.Teams.Any(e => e.Id == id);
        }
    }
}