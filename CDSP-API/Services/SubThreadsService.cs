using CDSP_API.Data;
using CDSP_API.Model;
using CDSP_API.Models;
using Microsoft.EntityFrameworkCore;
using System;
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

        public async Task<EnityCoreResult> CreateAsync(SubThread subThread, User user)
        {
            EnityCoreResult ecr = new EnityCoreResult();
            try
            {
                subThread.CreatorId = user.Id;
                await _dataContext.SubThread.AddAsync(subThread);
                await _dataContext.SaveChangesAsync();

                await Join(subThread, user, SubThreadRoleEnum.MODERATOR);
            }catch(Exception ex)
            {
                ecr.MapException(ex);
            }
            ecr.IsSuccess = ecr.ErrorMsg != null ? false : true;
            return ecr;
        }

        public async Task<EnityCoreResult> DeleteAsync(SubThread subThread)
        {
            EnityCoreResult ecr = new EnityCoreResult();

            try
            {
                await GetByNameAsync(subThread.Name);
                _dataContext.SubThread.Remove(subThread);
                await _dataContext.SaveChangesAsync();
            }
            catch(Exception ex)
            {
                ecr.MapException(ex);
            }
            
            await GetByNameAsync(subThread.Name);

            ecr.IsSuccess = ecr.ErrorMsg != null ? false : true;
            return ecr;
        }

        public async Task<(EnityCoreResult,List<SubThread>)> GetAllAsync()
        {
            EnityCoreResult ecr = new EnityCoreResult();
            List<SubThread> subThreads =null;
            try
            {
                subThreads = await _dataContext.SubThread.ToListAsync();
            }
            catch(Exception ex)
            {
                ecr.MapException(ex);
            }

            ecr.IsSuccess = ecr.ErrorMsg != null ? false : true;
            return (ecr, subThreads) ;
        }

        public async Task<(EnityCoreResult, SubThread)> GetByNameAsync(string name)
        {
            EnityCoreResult ecr = new EnityCoreResult();
            SubThread subThread = null;
            try
            {
                subThread =  await _dataContext.SubThread.SingleOrDefaultAsync(r => r.Name == name);
            }
            catch(Exception ex)
            {
                ecr.MapException(ex);
            }
            ecr.IsSuccess = ecr.ErrorMsg!=null? false: true;
            return (ecr, subThread);
        }

        public async Task<EnityCoreResult> UpdateAsync(SubThread subThread)
        {
            EnityCoreResult ecr = new EnityCoreResult();

            try
            {
                _dataContext.SubThread.Update(subThread);
                await _dataContext.SaveChangesAsync();
            }
            catch(Exception ex)
            {
                ecr.MapException(ex);
            }

            ecr.IsSuccess = ecr.ErrorMsg == null? false: true;
            return ecr;
        }

        public async Task<EnityCoreResult> Join(SubThread subThread, User user, SubThreadRoleEnum subThreadRoleEnum)
        {
            EnityCoreResult ecr = new EnityCoreResult();
            SubThreadUser subThreadUser; 

            try
            {
                subThreadUser = new SubThreadUser
                {
                    SubThreadId = subThread.Id,
                    UserId = user.Id,
                    SubThreadRoleId = (int)subThreadRoleEnum,
                };

                await _dataContext.SubThreadUser.AddAsync(subThreadUser);
                await _dataContext.SaveChangesAsync();
            }
            catch(Exception ex)
            {
                ecr.MapException(ex);
            }

            ecr.IsSuccess = ecr.ErrorMsg == null ? false : true;
            return ecr;
        }

        public async Task<EnityCoreResult> Leave(SubThread subThread, User user)
        {
            EnityCoreResult ecr = new EnityCoreResult();
            SubThreadUser subThreadUser;

            try
            {
                subThreadUser = await _dataContext.SubThreadUser.SingleOrDefaultAsync(r => r.SubThreadRoleId == subThread.Id && r.UserId == user.Id);
                _dataContext.SubThreadUser.Remove(subThreadUser);
                int rowsAffected = await _dataContext.SaveChangesAsync();
            }
            catch(Exception ex)
            {
                ecr.MapException(ex);
            }

            ecr.IsSuccess = ecr.ErrorMsg == null ? false : true;
            return ecr;
        }

        public async Task<EnityCoreResult> IsUserMember(SubThread subThread, User user)
        {
            EnityCoreResult ecr = new EnityCoreResult();
            SubThreadUser subThreadUser;

            try
            {
                subThreadUser = await _dataContext.SubThreadUser.SingleOrDefaultAsync(r => r.UserId == user.Id && r.SubThreadId == subThread.Id);
            }
            catch(Exception ex)
            {
                ecr.MapException(ex);
            }
            
            ecr.IsSuccess = ecr.ErrorMsg == null ? false : true;
            return ecr;
        }


    }
}
