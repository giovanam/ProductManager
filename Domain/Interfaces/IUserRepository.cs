using ProductManager.Domain.Entities;

namespace ProductManager.Domain.Interfaces
{
    public interface IUserRepository
    {
        User GetUserByEmail(string email);
        void AddUser(User user);
    }
}
