using Back_End.Models;

namespace Back_End.IRepository;

public interface IUserRepository
{
    public Task<User?> GetUserByUsernameAsync(string username);
    public Task<User?> GetUserByIdAsync(int id);
    public Task AddUserAsync(User user);
}
