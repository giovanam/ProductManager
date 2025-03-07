using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using ProductManager.Application.DTOs;
using ProductManager.Domain.Entities;
using ProductManager.Domain.Interfaces;

namespace ProductManager.Application.Services
{
    public class AuthService : IAuthService
    {
        private readonly IUserRepository _userRepository;
        private readonly IConfiguration _configuration;
        private readonly ILogger<AuthService> _logger;

        public AuthService(IUserRepository userRepository, IConfiguration configuration, ILogger<AuthService> logger)
        {
            _userRepository = userRepository;
            _configuration = configuration;
            _logger = logger;
        }

        public string Login(LoginDTO loginDTO)
        {
            try
            {
                _logger.LogInformation("Tentativa de login para o e-mail: {Email}", loginDTO.Email);
                var user = _userRepository.GetUserByEmail(loginDTO.Email);
                if (user == null || !BCrypt.Net.BCrypt.Verify(loginDTO.Password, user.PasswordHash))
                {
                    _logger.LogWarning("Falha no login para o e-mail: {Email}", loginDTO.Email);
                    return null;
                }

                _logger.LogInformation("Login bem-sucedido para o e-mail: {Email}", loginDTO.Email);
                return GenerateJwtToken(user);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao realizar login para o e-mail: {Email}", loginDTO.Email);
                throw;
            }
        }

        public bool Register(RegisterDTO registerDTO)
        {
            try
            {
                _logger.LogInformation("Tentativa de registro para o e-mail: {Email}", registerDTO.Email);
                if (_userRepository.GetUserByEmail(registerDTO.Email) != null)
                {
                    _logger.LogWarning("Registro falhou: e-mail já cadastrado: {Email}", registerDTO.Email);
                    return false;
                }

                var user = new User
                {
                    Email = registerDTO.Email,
                    PasswordHash = BCrypt.Net.BCrypt.HashPassword(registerDTO.Password)
                };

                _userRepository.AddUser(user);
                _logger.LogInformation("Usuário registrado com sucesso: {Email}", registerDTO.Email);
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao registrar usuário para o e-mail: {Email}", registerDTO.Email);
                throw;
            }
        }

        private string GenerateJwtToken(User user)
        {
            try
            {
                _logger.LogInformation("Gerando token JWT para o e-mail: {Email}", user.Email);
                var tokenHandler = new JwtSecurityTokenHandler();
                var key = Encoding.ASCII.GetBytes(_configuration["Jwt:Key"]);
                var tokenDescriptor = new SecurityTokenDescriptor
                {
                    Subject = new ClaimsIdentity(new[] { new Claim(ClaimTypes.Email, user.Email) }),
                    Expires = DateTime.UtcNow.AddHours(1),
                    SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
                };

                var token = tokenHandler.CreateToken(tokenDescriptor);
                return tokenHandler.WriteToken(token);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao gerar token JWT para o e-mail: {Email}", user.Email);
                throw;
            }
        }
    }
}
