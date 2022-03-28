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
    public class FeedService : IFeedService
    {
        private readonly DataContext _dataContext;
        private readonly IUsersService _usersService;
        public FeedService(DataContext dataContext, IUsersService usersService)
        {
            _dataContext = dataContext;
            _usersService = usersService;
        }

        public Task<(EnityCoreResult, List<Post>)> Feed()
        {
            EnityCoreResult ecr = new EnityCoreResult();
            try
            {
            }catch (Exception ex)
            {
                ecr.IsSuccess = false;
                ecr.MapException(ex);
            }
            throw new NotImplementedException();
        }

        public Task<(EnityCoreResult, List<Comment>)> GetLoggedUserComments(int id)
        {
            throw new NotImplementedException();
        }

        public Task<(EnityCoreResult, List<Post>)> GetLoggedUserFeed()
        {
            throw new NotImplementedException();
        }

        public Task<(EnityCoreResult, List<Post>)> GetLoggedUserPosts(int id)
        {
            throw new NotImplementedException();
        }
    }
}