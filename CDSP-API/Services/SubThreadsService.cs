using CDSP_API.Data;
using CDSP_API.Model;
using CDSP_API.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CDSP_API.Services
{
    public class SubThreadsService : ISubThreadsService
    {
        private readonly DataContext _dataContext;

        public SubThreadsService(DataContext dataContext)
        {
            _dataContext = dataContext;
        }

        public async Task<bool> CreateAsync(SubThread subThread, User user)
        {
            subThread.CreatorId = user.Id;
            await _dataContext.SubThread.AddAsync(subThread);
            var rowsAffected = await _dataContext.SaveChangesAsync();

            await Join(subThread, user, SubThreadRoleEnum.MODERATOR);
            return rowsAffected > 0;
        }

        public async Task<bool> DeleteAsync(SubThread subThread)
        {
            await GetByNameAsync(subThread.Name);

            _dataContext.SubThread.Remove(subThread);
            var rowsAffected = await _dataContext.SaveChangesAsync();

            return rowsAffected > 0;
        }

        public async Task<List<SubThread>> GetAllAsync()
        {
            return await _dataContext.SubThread.ToListAsync();
        }

        public async Task<SubThread> GetByNameAsync(string name)
        {
            return await _dataContext.SubThread.SingleOrDefaultAsync(r => r.Name == name);
        }

        public async Task<bool> UpdateAsync(SubThread subThread)
        {
            _dataContext.SubThread.Update(subThread);
            int rowsEffected = await _dataContext.SaveChangesAsync();

            return (rowsEffected > 0);
        }

        public async Task<bool> Join(SubThread subThread, User user, SubThreadRoleEnum subThreadRoleEnum)
        {
            SubThreadUser subThreadUser = new SubThreadUser
            {
                SubThreadId = subThread.Id,
                UserId = user.Id,
                SubThreadRoleId = (int)subThreadRoleEnum,
            };

            await _dataContext.SubThreadUser.AddAsync(subThreadUser);
            int rowsAffected = await _dataContext.SaveChangesAsync();

            return rowsAffected > 0;
        }

        public async Task<bool> Leave(SubThread subThread, User user)
        {
            SubThreadUser subThreadUser = await _dataContext.SubThreadUser.SingleOrDefaultAsync(r=> r.SubThreadRoleId==subThread.Id && r.UserId==user.Id);
            
            _dataContext.SubThreadUser.Remove(subThreadUser);
            int rowsAffected = await _dataContext.SaveChangesAsync();

            return rowsAffected > 0;
        }

        public async Task<bool> IsUserMember(SubThread subThread, User user)
        {
            SubThreadUser subThreadUser = await _dataContext.SubThreadUser.SingleOrDefaultAsync(r=>r.UserId==user.Id && r.SubThreadId==subThread.Id);
            return subThreadUser is null? false: true;
        }

    }
}
