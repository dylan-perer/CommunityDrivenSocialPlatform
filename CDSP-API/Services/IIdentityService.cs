using CDSP_API.Contracts.V1.Requests;
using System.Threading.Tasks;
using CDSP_API.Models;
using CDSP_API.Data;

namespace CDSP_API.Services
{
    public interface IIdentityService
    {
        public Task<AuthResult> SigninAsync(User user);
        public Task<AuthResult> SignupAsync(User user);
        public Task<AuthResult> GenerateToken(User user);
        public Task<AuthResult> VerifyAndGenerateToken(string token, string refreshToken);
        public string HashPassword(string password);
        public bool VerifyPassword(string password, string passwordHash);
    }
}
