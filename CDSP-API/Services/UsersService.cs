using CDSP_API.Data;
using CDSP_API.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace CDSP_API.Services
{
    public class UsersService : IUsersService
    {
        private readonly DataContext DataContext;

        public UsersService(DataContext dataContext)
        {
            DataContext = dataContext;
        }

        public async Task<bool> CreateAsync(User user)
        {
            await DataContext.User.AddAsync(user);
            var rowsAffected =  await DataContext.SaveChangesAsync();

            return rowsAffected > 0;
        }

        public async Task<bool> DeleteAsync(User user)
        {
            await GetByUsernameAsync(user.Username);

            DataContext.User.Remove(user);
            var rowsAffected = await DataContext.SaveChangesAsync();

            return rowsAffected > 0;
        }

        public async Task<List<User>> GetAllAsync()
        {
            return await DataContext.User.ToListAsync();
        }

        public async Task<User> GetByUsernameAsync(string username)
        {
            return await DataContext.User.SingleOrDefaultAsync(r => r.Username == username);
        }

        public async Task<User> GetLoggedUser(ClaimsPrincipal claimsPrincipal)
        {
            string loggedUser = claimsPrincipal.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;
            return await DataContext.User.FirstOrDefaultAsync(r => r.Username == loggedUser);
        }

        public async Task<bool>UpdateAsync(User user)
        {
            DataContext.User.Update(user);
            int rowsEffected = await DataContext.SaveChangesAsync();

            return (rowsEffected > 0);
        }
    }
}
