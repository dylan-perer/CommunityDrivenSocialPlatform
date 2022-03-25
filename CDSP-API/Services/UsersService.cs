using CDSP_API.Data;
using CDSP_API.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace CDSP_API.Services
{
    public class UsersService : IUsersService
    {
        private readonly DataContext _dataContext;

        public UsersService(DataContext dataContext)
        {
            _dataContext = dataContext;
        }

        public async Task<(EnityCoreResult, User)> CreateAsync(User user)
        {
            EnityCoreResult ecr = new EnityCoreResult();
            try
            {
                await _dataContext.User.AddAsync(user);
                await _dataContext.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                ecr.MapException(ex);
            }
            
            ecr.IsSuccess = ecr.ErrorMsg != null ? false : true;
            return (ecr, user);
        }

        public async Task<EnityCoreResult> DeleteAsync(User user)
        {
            EnityCoreResult ecr = new EnityCoreResult();
            try
            {
                await GetByUsernameAsync(user.Username);

                _dataContext.User.Remove(user);
                 await _dataContext.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                ecr.MapException(ex);
            }

            ecr.IsSuccess = ecr.ErrorMsg != null ? false : true;
            return ecr;
        }

        public async Task<(EnityCoreResult, List<User>)> GetAllAsync()
        {
            EnityCoreResult ecr = new EnityCoreResult();
            List<User> users = null;
            try
            {
                users = await _dataContext.User.ToListAsync();
            }
            catch (Exception ex)
            {
                ecr.MapException(ex);
            }
            ecr.IsSuccess = ecr.ErrorMsg != null ? false : true;
            return (ecr, users);
        }

        public async Task<(EnityCoreResult, User)> GetByUsernameAsync(string username)
        {
            EnityCoreResult ecr = new EnityCoreResult();
            User user = null;
            try
            {
                user = await _dataContext.User.SingleOrDefaultAsync(r => r.Username == username);
            }
            catch (Exception ex)
            {
                ecr.MapException(ex);
            }
            ecr.IsSuccess = ecr.ErrorMsg!=null? false: true;
            return (ecr, user);
        }

        public async Task<(EnityCoreResult, User)> GetLoggedUser(ClaimsPrincipal claimsPrincipal)
        {
            EnityCoreResult ecr = new EnityCoreResult();
            User user = null;
            try
            {
                string loggedUser = claimsPrincipal.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;
                user = await _dataContext.User.FirstOrDefaultAsync(r => r.Username == loggedUser);
            }
            catch (Exception ex)
            {
                ecr.MapException(ex);
            }
            ecr.IsSuccess = ecr.ErrorMsg != null ? false : true;
            return (ecr, user);
        }

        public async Task<EnityCoreResult> UpdateAsync(User user)
        {
            EnityCoreResult ecr = new EnityCoreResult();
            try
            {
                _dataContext.User.Update(user);
                  await _dataContext.SaveChangesAsync();
            }
            catch(Exception ex)
            {
                ecr.MapException(ex);
            }

            ecr.IsSuccess = ecr.ErrorMsg != null ? false : true;
            return ecr;
        }
    }
}
