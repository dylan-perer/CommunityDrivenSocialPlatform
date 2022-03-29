using CDSP_API.Contracts.V1.Requests;
using System.Threading.Tasks;
using CDSP_API.Models;
using CDSP_API.Data;
using System.Collections.Generic;

namespace CDSP_API.Services
{
    public interface ICommentService
    {
        public Task<(EnityCoreResult, Comment)> GetByIdAsync(int id);
        public Task<(EnityCoreResult, List<Comment>)> GetAll(int PostId);
        public Task<(EnityCoreResult, Comment)> CreateAsync(Comment comment, User user);
        public Task<(EnityCoreResult, Comment)> UpdateAsync(Comment comment, User user);
        public Task<EnityCoreResult> DeleteByIdAsync(int id, User user);
    }
}
