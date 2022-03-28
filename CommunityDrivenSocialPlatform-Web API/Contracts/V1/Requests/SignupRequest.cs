using CDSP_API.Model;
using CDSP_API.Models;
using CDSP_API.Validation;
using System;
using System.ComponentModel.DataAnnotations;

namespace CDSP_API.Contracts.V1.Requests
{
    public class SignupRequest
    {
        [Required]
        [EnsureUqniqueUsername]
        public string Username { get; set; }

        [Required]
        [EnsureUniqueEmailAddress]
        public string EmailAddress { get; set; }   
        
        [Required]
        [EnsureValidPassword]
        public string Password { get; set; }

        public User MapToModel()
        {
            return new User
            {
                Username = Username,
                EmailAddress = EmailAddress,    
                Password = Password,
                RoleId = (int)UserRoleEnum.USER,
                CreatedAt = DateTime.Now,
            };
        }

        public override string ToString()
        {
            return $"Username: {Username}, EmailAdress: {EmailAddress}, Password: {Password}";
        }


    }
}
