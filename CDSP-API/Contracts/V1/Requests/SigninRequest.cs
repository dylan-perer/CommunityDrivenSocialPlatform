using CDSP_API.Models;
using CDSP_API.Validation;
using System.ComponentModel.DataAnnotations;

namespace CDSP_API.Contracts.V1.Requests
{
    public class SigninRequest
    {
        [Required]
        public string Username { get; set; }

        [Required]
        [EnsureValidPassword]
        public string Password { get; set; }

        public User MapToModel()
        {
            return new User
            {
                Username = Username,
                Password = Password,
            };
        }
    }
}
