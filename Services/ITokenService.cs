using TaskManagerApi.Models;

namespace TaskManagerApi.Services
{
    public interface ITokenService
    {
        string GenerateToken(User user);
    }
}