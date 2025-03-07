using ProductManager.Domain.Entities;
using ProductManager.Domain.Interfaces;
using ProductManager.Infrastructure.Data;

namespace ProductManager.Infrastructure.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly AuthContext _context;
        private readonly ILogger<UserRepository> _logger;

        public UserRepository(AuthContext context, ILogger<UserRepository> logger)
        {
            _context = context;
            _logger = logger;
        }

        public User GetUserByEmail(string email)
        {
            try
            {
                _logger.LogInformation("Buscando usuário pelo e-mail: {Email}", email);
                return _context.Users.SingleOrDefault(u => u.Email == email);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao buscar usuário pelo e-mail: {Email}", email);
                throw;
            }
        }

        public void AddUser(User user)
        {
            try
            {
                _logger.LogInformation("Adicionando novo usuário com e-mail: {Email}", user.Email);
                _context.Users.Add(user);
                _context.SaveChanges();
                _logger.LogInformation("Usuário adicionado com sucesso: {Email}", user.Email);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao adicionar usuário com e-mail: {Email}", user.Email);
                throw;
            }
        }
    }
}
