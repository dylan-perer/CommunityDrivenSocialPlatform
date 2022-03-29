using CDSP_API.Data;
using CDSP_API.Model;
using CDSP_API.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
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
            }
            catch (Exception ex)
            {
                ecr.MapException(ex);
            }
            ecr.IsSuccess = ecr.ErrorMsg == null ? true : false;
            return ecr;
        }

        public async Task<EnityCoreResult> DeleteAsync(SubThread subThread, User user)
        {
            EnityCoreResult ecr = new EnityCoreResult();

            try
            {
                if(subThread.CreatorId == user.Id)
                {
                    await GetByNameAsync(subThread.Name);
                    _dataContext.SubThread.Remove(subThread);
                    await _dataContext.SaveChangesAsync();
                }
            }
            catch (Exception ex)
            {
                ecr.MapException(ex);
            }

            await GetByNameAsync(subThread.Name);

            ecr.IsSuccess = ecr.ErrorMsg == null ? true : false;
            return ecr;
        }

        public async Task<(EnityCoreResult, List<SubThread>)> GetAllAsync()
        {
            EnityCoreResult ecr = new EnityCoreResult();
            List<SubThread> subThreads = null;
            try
            {
                subThreads = await _dataContext.SubThread.ToListAsync();
            }
            catch (Exception ex)
            {
                ecr.MapException(ex);
            }

            ecr.IsSuccess = ecr.ErrorMsg == null ? true : false;
            return (ecr, subThreads);
        }

        public async Task<(EnityCoreResult, SubThread)> GetByNameAsync(string name)
        {
            EnityCoreResult ecr = new EnityCoreResult();
            SubThread subThread = null;
            try
            {
                subThread = await _dataContext.SubThread.SingleOrDefaultAsync(r => r.Name == name);
            }
            catch (Exception ex)
            {
                ecr.MapException(ex);
            }
            ecr.IsSuccess = ecr.ErrorMsg == null ? true : false;
            return (ecr, subThread);
        }

        public async Task<EnityCoreResult> UpdateAsync(SubThread subThread, User user)
        {
            EnityCoreResult ecr = new EnityCoreResult();

            try
            {
                SubThreadUser subThreadUser = await _dataContext.SubThreadUser.SingleOrDefaultAsync(r => r.UserId == user.Id && r.SubThreadId == subThread.Id);
                if (subThreadUser != null && subThreadUser.SubThreadRoleId==(int)SubThreadRoleEnum.MODERATOR)
                {
                    _dataContext.SubThread.Update(subThread);
                    await _dataContext.SaveChangesAsync();
                }
            }
            catch (Exception ex)
            {
                ecr.MapException(ex);
            }

            ecr.IsSuccess = ecr.ErrorMsg == null ? true : false;
            return ecr;
        }

        public async Task<EnityCoreResult> Join(SubThread subThread, User user, SubThreadRoleEnum subThreadRoleEnum)
        {
            EnityCoreResult ecr = new EnityCoreResult();
            SubThreadUser subThreadUser;
            int rowsAffected = 0;
            try
            {
                subThreadUser = new SubThreadUser
                {
                    SubThreadId = subThread.Id,
                    UserId = user.Id,
                    SubThreadRoleId = (int)subThreadRoleEnum,
                };

                await _dataContext.SubThreadUser.AddAsync(subThreadUser);
                rowsAffected = await _dataContext.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                ecr.MapException(ex);
            }

            ecr.IsSuccess = rowsAffected > 0;
            return ecr;
        }

        public async Task<EnityCoreResult> Leave(SubThreadUser subThreadUser, User user)
        {
            EnityCoreResult ecr = new EnityCoreResult();
            int rowsAffected = 0;

            try
            {
                _dataContext.SubThreadUser.Remove(subThreadUser);
                rowsAffected = await _dataContext.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                ecr.MapException(ex);
            }

            ecr.IsSuccess = rowsAffected > 0;
            return ecr;
        }

        public async Task<(EnityCoreResult, SubThreadUser)> IsUserMember(SubThread subThread, User user)
        {
            EnityCoreResult ecr = new EnityCoreResult();
            SubThreadUser subThreadUser = null;

            try
            {
                subThreadUser = await _dataContext.SubThreadUser.SingleOrDefaultAsync(r => r.UserId == user.Id && r.SubThreadId == subThread.Id);
            }
            catch (Exception ex)
            {
                ecr.MapException(ex);
            }
            ecr.IsSuccess = ecr.ErrorMsg == null ? true : false;
            return (ecr, subThreadUser);
        }

        public async Task<(EnityCoreResult, List<User>)> SubThreadUsersAsync(string name)
        {
            EnityCoreResult ecr = new EnityCoreResult();
            List<User> users = new List<User>();
            try
            {
                (var subthreadEcr, SubThread subThread) = await GetByNameAsync(name);
                var subthreadUsers = await _dataContext.SubThreadUser.Where(r => r.SubThreadId == subThread.Id).ToListAsync();

                var innerJoin = _dataContext.User.Join(_dataContext.SubThreadUser, tbl_user => tbl_user.Id, tbl_sub_thread_user => tbl_sub_thread_user.UserId,
                (tbl_user, tbl_sub_thread_user) => new { tbl_user, tbl_sub_thread_user })
                .Select(c => new { c.tbl_user.Username, c.tbl_sub_thread_user.SubThreadRoleId, c.tbl_sub_thread_user.SubThreadId, c.tbl_user.Description, c.tbl_user.Id, c.tbl_user.ProfilePictureUrl })
                .Where(r => r.SubThreadId == subThread.Id);

                foreach (var user in innerJoin)
                {
                    users.Add(new User {
                        Id = user.Id,
                        Username = user.Username,
                        Description = user.Description,
                        ProfilePictureUrl = user.ProfilePictureUrl,
                    });
                }
            }
            catch (Exception ex)
            {
                ecr.MapException(ex);
            }
            ecr.IsSuccess = ecr.ErrorMsg == null ? true : false;
            return (ecr, users);
        }
    }
}
