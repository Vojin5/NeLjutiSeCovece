using Back_End.IRepository;
using Back_End.Models;
using Microsoft.EntityFrameworkCore;

namespace Back_End.Repository;

public class UserRepository : IUserRepository
{
    public Context Context { get; set; }
    public UserRepository(Context context)
    {
        Context = context;
    }
    public async Task AddUserAsync(User user)
    {
        await Context.Users.AddAsync(user);
        await Context.SaveChangesAsync();
    }

    public async Task<User?> GetUserByUsernameAsync(string username)
    {
        var existingUser = await Context.Users
            .Where(u => u.Username == username)
            .FirstOrDefaultAsync();

        return existingUser;
    }

    public async Task<User?> GetUserByIdAsync(int id)
    {
        return await Context.Users
            .FindAsync(id);
    }
}
