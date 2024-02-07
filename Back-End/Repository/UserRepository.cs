using Back_End.IRepository;
using Back_End.Models;
using Microsoft.EntityFrameworkCore;

namespace Back_End.Repository;

public class UserRepository : IUserRepository
{
    public required Context Context { get; set; }
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

    public async Task<UserInfoViewModel> GetUserProfileInfoAsync(int id)
    {
        return await Context.Users
            .Where(u => u.Id == id)
            .Select(u => new UserInfoViewModel(u.Username, u.Password, u.Email, u.Image))
            .FirstOrDefaultAsync();
    }

    public async Task UpdateUserProfileInfoAsync(User user, UserUpdateModel userUpdate)
    {
        string salt = BCrypt.Net.BCrypt.GenerateSalt();
        string passwordHash = BCrypt.Net.BCrypt.HashPassword(userUpdate.Password, salt);

        user.Username = userUpdate.Username;
        user.Email = userUpdate.Email;
        user.Image = userUpdate.Image;
        user.Password = passwordHash;
        user.PasswordSalt = salt;

        Context.Users.Update(user);
        await Context.SaveChangesAsync();
        return;
       
    }
}
