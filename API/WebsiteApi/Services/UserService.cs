using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using WebsiteApi.Context;
using WebsiteApi.Models;

namespace WebsiteApi.Services;

public class UserService(DataContext context)
{
    private readonly DataContext _context = context;

    public async Task<User?> GetUserAsync(int userId)
    {
        return await _context.Users.FindAsync(userId);
    }

    public async Task<User?> GetUserByEmail(string email)
    {
        return await _context.Users.FirstOrDefaultAsync(x => x.Email == email);
    }


    public async Task<List<User>> GetAllUsers()
    {
        return await _context.Users.ToListAsync();
    }

    public async Task AddNewUser(User user)
    {
        await _context.Users.AddAsync(user);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateUser(User user)
    {
        _context.Users.Update(user);
        await _context.SaveChangesAsync();
    }

    public async Task<bool> DeleteUser(User user)
    {
        var userToDelete = await _context.Users.FindAsync(user);
        if (userToDelete == null) return false;

        _context.Users.Remove(user);
        await _context.SaveChangesAsync();
        return true;
    }
}