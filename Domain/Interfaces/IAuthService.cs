using ProductManager.Application.DTOs;

namespace ProductManager.Domain.Interfaces
{
    public interface IAuthService
    {
        string Login(LoginDTO loginModel);
        bool Register(RegisterDTO registerModel);
    }
}
