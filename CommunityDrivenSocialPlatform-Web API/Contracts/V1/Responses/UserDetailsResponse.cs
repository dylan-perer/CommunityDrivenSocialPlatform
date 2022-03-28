using CDSP_API.Models;
using System.Collections.Generic;

namespace CDSP_API.Contracts.V1.Responses
{
    public class UserDetailsResponse
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public string Description { get; set; }
        public string ProfilePictureUrl { get; set; }

        public UserDetailsResponse MapToReponse(User user)
        {
            return new UserDetailsResponse
            {
                Id = user.Id,
                Username = user.Username,
                Description = user.Description,
                ProfilePictureUrl = user.ProfilePictureUrl,
            };
        }

        public List<UserDetailsResponse> MapToReponse(List<User> users)
        {
            List<UserDetailsResponse> userDetailsResponses = new List<UserDetailsResponse>();

            foreach (var user in users)
            {
                userDetailsResponses.Add(new UserDetailsResponse().MapToReponse(user));
            }

            return userDetailsResponses;
        }
    }
}
