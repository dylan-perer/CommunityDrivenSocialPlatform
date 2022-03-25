using CDSP_API.Data;
using CDSP_API.Models;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;

namespace CDSP_API.Services
{
    public interface IUsersService
    {
        public Task<(EnityCoreResult, User)> CreateAsync(User user);
        public Task<(EnityCoreResult,User)> GetByUsernameAsync(string username);
        public Task<(EnityCoreResult,List<User>)> GetAllAsync();
        public Task<EnityCoreResult> UpdateAsync(User user);
        public Task<EnityCoreResult> DeleteAsync(User user);
        public Task<(EnityCoreResult,User)> GetLoggedUser(ClaimsPrincipal claimsPrincipal);
    }
}
