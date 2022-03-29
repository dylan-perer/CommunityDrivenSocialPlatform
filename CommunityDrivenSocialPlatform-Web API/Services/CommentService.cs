using CDSP_API.Data;
using CDSP_API.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Text;
using CDSP_API.Models;
using System.Threading.Tasks;
using System.Linq;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;
using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;

namespace CDSP_API.Services
{
    public class CommentService : ICommentService
    {
        private readonly DataContext _dataContext;
        private readonly IUsersService _usersService;
        public CommentService(DataContext dataContext, IUsersService usersService)
        {
            _dataContext = dataContext;
            _usersService = usersService;
        }

        public async Task<(EnityCoreResult, Comment)> CreateAsync(Comment comment, User user)
        {
            EnityCoreResult ecr = new EnityCoreResult();
            try
            {
                comment.UserId = user.Id;
                await _dataContext.Comment.AddAsync(comment);
                await _dataContext.SaveChangesAsync();
            }catch (Exception ex)
            {
                ecr.IsSuccess = false;
                ecr.MapException(ex);
            }
            return (ecr, comment); 
        }

        public async Task<EnityCoreResult> DeleteByIdAsync(int id, User user)
        {
            EnityCoreResult ecr = new EnityCoreResult();
            try
            {
                (var _ecr, var comment) = await GetByIdAsync(id);
                if(comment.UserId == user.Id)
                {
                    _dataContext.Comment.Remove(comment);
                    await _dataContext.SaveChangesAsync();
                }
            }
            catch (Exception ex)
            {
                ecr.IsSuccess = false;
                ecr.MapException(ex);
            }
            return ecr;
        }

        public async Task<(EnityCoreResult, List<Comment>)> GetAll(int PostId)
        {
            EnityCoreResult ecr = new EnityCoreResult();
            List<Comment> comments = null;
            try
            {   
                comments = await _dataContext.Comment.Where(r=>r.PostId == PostId).ToListAsync();
            }
            catch (Exception ex)
            {
                ecr.IsSuccess = false;
                ecr.MapException(ex);
            }
            return (ecr, comments);
        }

        public async Task<(EnityCoreResult, Comment)> GetByIdAsync(int id)
        {
            EnityCoreResult ecr = new EnityCoreResult();
            Comment comment = null;
            try
            {
                comment = await _dataContext.Comment.SingleOrDefaultAsync(r => r.Id == id);
            }
            catch (Exception ex)
            {
                ecr.IsSuccess = false;
                ecr.MapException(ex);
            }
            return (ecr, comment);
        }

        public async Task<(EnityCoreResult, Comment)> UpdateAsync(Comment comment, User user)
        {
            EnityCoreResult ecr = new EnityCoreResult();
            try
            {
                (var _ecr, var _comment) = await GetByIdAsync(comment.Id);
                if (comment.UserId == user.Id)
                {
                    _comment.Body = comment.Body;
                    _dataContext.Comment.Update(_comment);
                    await _dataContext.SaveChangesAsync();
                }

            }
            catch (Exception ex)
            {
                ecr.IsSuccess = false;
                ecr.MapException(ex);
            }
            return (ecr, comment);
        }
    }
}