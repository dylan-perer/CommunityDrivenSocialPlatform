using CDSP_API.Contracts.V1.Requests;
using System.Threading.Tasks;
using CDSP_API.Models;
using CDSP_API.Data;
using System.Collections.Generic;

namespace CDSP_API.Services
{
    public interface IPostService
    {
        public Task<(EnityCoreResult, Post)> GetByIdAsync(int id);
        public Task<(EnityCoreResult, List<Post>)> GetAllPostsOnSubThreadAsync(string subthreadName);
        public Task<(EnityCoreResult, Post)> CreateAsync(Post post, User user, string subThreadName);
        public Task<(EnityCoreResult, Post)> UpdateAsync(Post post, User user);
        public Task<EnityCoreResult> DeleteByIdAsync(int id, User user);
        public Task<EnityCoreResult> AddUpVoteAsync(int id, User user);
        public Task<EnityCoreResult> AddDownVoteAsync(int id, User user);
        public Task<(EnityCoreResult, int)> GetVoteCountAsync(int id);
        public Task<(EnityCoreResult, List<Comment>)> GetAllCommentsAsync(int id);
    }
}
