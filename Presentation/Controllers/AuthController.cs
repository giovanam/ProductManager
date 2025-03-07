using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using ProductManager.Application.DTOs;
using ProductManager.Domain.Interfaces;

namespace AuthApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;
        private readonly ILogger<AuthController> _logger;

        public AuthController(IAuthService authService, ILogger<AuthController> logger)
        {
            _authService = authService;
            _logger = logger;
        }

        [HttpPost("login")]
        public IActionResult Login([FromBody] LoginDTO loginDTO)
        {
            try
            {
                _logger.LogInformation("Login attempt with email: {Email}", loginDTO.Email);

                var token = _authService.Login(loginDTO);

                if (token == null)
                {
                    _logger.LogWarning("Login failed for email: {Email}", loginDTO.Email);
                    return Unauthorized("Invalid credentials.");
                }

                _logger.LogInformation("Login successful for email: {Email}", loginDTO.Email);
                return Ok(new { token });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error during login for email: {Email}", loginDTO.Email);
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpPost("register")]
        public IActionResult Register([FromBody] RegisterDTO registerDTO)
        {
            try
            {
                _logger.LogInformation("Registration attempt for email: {Email}", registerDTO.Email);

                var result = _authService.Register(registerDTO);
                if (!result)
                {
                    _logger.LogWarning("Registration failed for email: {Email}", registerDTO.Email);
                    return BadRequest("Registration failed.");
                }

                _logger.LogInformation("Registration successful for email: {Email}", registerDTO.Email);
                return Ok("User registered.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error during registration for email: {Email}", registerDTO.Email);
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpGet("google")]
        public IActionResult GoogleLogin()
        {
            _logger.LogInformation("Google login initiated.");
            var redirectUrl = Url.Action(nameof(GoogleResponse), "Auth");
            var properties = new AuthenticationProperties { RedirectUri = redirectUrl };
            return Challenge(properties, "Google");
        }

        [HttpGet("google-response")]
        public async Task<IActionResult> GoogleResponse()
        {
            try
            {
                var info = await HttpContext.AuthenticateAsync("Google");
                var token = info.Properties.GetTokenValue("access_token");

                if (token == null)
                {
                    _logger.LogWarning("Google authentication failed.");
                    return Unauthorized("Authentication failed.");
                }

                _logger.LogInformation("Google authentication successful.");
                return Ok(new { token });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error during Google authentication.");
                return StatusCode(500, "Internal server error");
            }
        }
    }
}
