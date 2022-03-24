using CDSP_API.Models;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;

namespace CDSP_API.Services
{
    public interface IUsersService
    {
        public Task<bool> CreateAsync(User user);
        public Task<User> GetByUsernameAsync(string username);
        public Task<List<User>> GetAllAsync();
        public Task<bool> UpdateAsync(User user);
        public Task<bool> DeleteAsync(User user);
        public Task<User> GetLoggedUser(ClaimsPrincipal claimsPrincipal);
    }
}
