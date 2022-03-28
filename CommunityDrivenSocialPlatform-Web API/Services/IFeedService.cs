using CDSP_API.Contracts.V1.Requests;
using System.Threading.Tasks;
using CDSP_API.Models;
using CDSP_API.Data;
using System.Collections.Generic;

namespace CDSP_API.Services
{
    public interface IFeedService
    {
        public Task<(EnityCoreResult, List<Post>)> Feed();
        public Task<(EnityCoreResult, List<Post>)> GetLoggedUserFeed();
        public Task<(EnityCoreResult, List<Comment>)> GetLoggedUserComments(int id);
        public Task<(EnityCoreResult, List<Post>)> GetLoggedUserPosts(int id);
    }
}
