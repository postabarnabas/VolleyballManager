using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using VolleyballManager.Services;

namespace VolleyballManager.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly TokenService _tokenService;

        public AuthController(
            UserManager<IdentityUser> userManager,
            SignInManager<IdentityUser> signInManager,
            TokenService tokenService)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _tokenService = tokenService;
        }

        // ========================
        //   REGISTRATION
        // ========================
        [HttpPost("register")]
        public async Task<IActionResult> Register(RegisterDto model)
        {
            var user = new IdentityUser
            {
                UserName = model.Email,
                Email = model.Email
            };

            var result = await _userManager.CreateAsync(user, model.Password);

            if (!result.Succeeded)
                return BadRequest(result.Errors);
            await _userManager.AddToRoleAsync(user, "User");
            return Ok(new { message = "Registration successful" });
        }

        // ========================
        //   LOGIN
        // ========================
        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginDto model)
        {
            var user = await _userManager.FindByEmailAsync(model.Email);

            if (user == null)
                return Unauthorized("Invalid email or password");

            var result = await _signInManager.CheckPasswordSignInAsync(user, model.Password, false);

            if (!result.Succeeded)
                return Unauthorized("Invalid email or password");

            var roles = await _userManager.GetRolesAsync(user);

            var token = _tokenService.CreateToken(user, roles);

            return Ok(new { token });
        }
    }

    // ================
    // DTO CLASSES
    // ================
    public class RegisterDto
    {
        public string Email { get; set; } = default!;
        public string Password { get; set; } = default!;
    }

    public class LoginDto
    {
        public string Email { get; set; } = default!;
        public string Password { get; set; } = default!;
    }
}
