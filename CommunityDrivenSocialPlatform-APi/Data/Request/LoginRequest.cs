using CommunityDrivenSocialPlatform_APi.Data;
using CommunityDrivenSocialPlatform_APi.Validaton;
using System;

namespace CommunityDrivenSocialPlatform_APi.Model.Requests
{
    public class LoginRequest
    {
        [SignupEnsureUniqueUsername]
        public string Username { get; set; }
        public string Password { get; set; }

        [SignupEnsureUniqueEmail]
        public string EmailAddress { get; set; }
        public string ProfilePictureUrl { get; set; } = null;

        public User createUser()
        {
            return new User
            {//map usersignup object to user model
                Username = this.Username,
                Password = this.Password,
                EmailAddress = this.EmailAddress,
                ProfilePictureUrl = this.ProfilePictureUrl,

                CreatedAt = DateTime.Now,
                RoleId = (int)RoleEnum.USER
            };
        }
    }
}
