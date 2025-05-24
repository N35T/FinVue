using FinVue.Core.Entities;
using FinVue.Core.Exceptions;
using FinVue.Core.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace FinVue.Core.Services;
public class UserService {
    private readonly IApplicationDbContext _dbContext;

    public UserService(IApplicationDbContext dbContext) {
        _dbContext = dbContext;
    }
    public async Task<User?> GetUserFromIdAsync(string userId) {
        return await _dbContext.Users.FindAsync(userId);
    }
    public Task<List<User>> GetAllUsersAsync() {
        return _dbContext.Users.ToListAsync();
    }

    public async Task<User> AddUserAsync(User user) {
        _dbContext.Users.Add(user);
        var changedRows = await _dbContext.SaveChangesAsync();

        if (changedRows > 0) {
            return user;
        }

        throw new UserException("Failed adding the user: \n" + user.ToString());
    }
}

