using CDSP_API.Models;
using CDSP_API.Validation;

namespace CDSP_API.Contracts.V1.Requests
{
    public class UpdateUserDetailsRequest
    {
        public string Description { get; set; }
        public string ProfilePictureUrl { get; set; }

        [EnsureValidPassword]
        public string Password { get; set; }

        public User MapToModel(User user)
        {
            if(Description != null) 
                user.Description = Description;
            if(ProfilePictureUrl != null)
                user.ProfilePictureUrl = ProfilePictureUrl;
            if(Password != null)
                user.Password = Password;
            return user;
        }

        public override string ToString()
        {
            return $"Description: {Description}, ProfilePictureUrl: {ProfilePictureUrl}";
        }
    }
}
