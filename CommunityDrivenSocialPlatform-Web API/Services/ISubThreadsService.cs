using CDSP_API.Data;
using CDSP_API.Model;
using CDSP_API.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CDSP_API.Services
{
    public interface ISubThreadsService
    {
        public Task<EnityCoreResult> CreateAsync(SubThread subThread, User user);
        public Task<(EnityCoreResult,SubThread)> GetByNameAsync(string name);
        public Task<(EnityCoreResult,List<SubThread>)> GetAllAsync();
        public Task<EnityCoreResult> UpdateAsync(SubThread subThread, User user);
        public Task<EnityCoreResult> DeleteAsync(SubThread subThread, User user);
        public Task<EnityCoreResult> Join(SubThread subThread, User user, SubThreadRoleEnum subThreadRoleEnum);
        public Task<(EnityCoreResult, SubThreadUser)> IsUserMember(SubThread subThread, User user);
        public Task<EnityCoreResult> Leave(SubThreadUser subThreadUser, User user);
        public Task<(EnityCoreResult, List<User>)> SubThreadUsersAsync(string name);
    }
}
