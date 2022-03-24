using CDSP_API.Model;
using CDSP_API.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CDSP_API.Services
{
    public interface ISubThreadsService
    {
        public Task<bool> CreateAsync(SubThread subThread, User user);
        public Task<SubThread> GetByNameAsync(string name);
        public Task<List<SubThread>> GetAllAsync();
        public Task<bool> UpdateAsync(SubThread subThread);
        public Task<bool> DeleteAsync(SubThread subThread);
        public Task<bool> Join(SubThread subThread, User user, SubThreadRoleEnum subThreadRoleEnum);
        public Task<bool> IsUserMember(SubThread subThread, User user);
        public Task<bool> Leave(SubThread subThread, User user);
    }
}
