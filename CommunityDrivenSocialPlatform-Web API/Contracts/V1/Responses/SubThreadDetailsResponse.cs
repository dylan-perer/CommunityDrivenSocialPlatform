using CDSP_API.Models;
using System.Collections.Generic;

namespace CDSP_API.Contracts.V1.Responses
{
    public class SubThreadDetailsResponse
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string WelcomeMessage { get; set; }
        public int CreatorId { get; set; }

        public SubThreadDetailsResponse MapToReponse(SubThread subThread)
        {
            return new SubThreadDetailsResponse
            {
                Id = subThread.Id,
                Name = subThread.Name,
                Description = subThread.Description,
                WelcomeMessage = subThread.WelcomeMessage,
                CreatorId = subThread.CreatorId
            };
        }

        public List<SubThreadDetailsResponse> MapToReponse(List<SubThread> subThreads)
        {
            List<SubThreadDetailsResponse> subThreadDetailsResponses = new List<SubThreadDetailsResponse>();

            foreach (var subThread in subThreads)
            {
                subThreadDetailsResponses.Add(new SubThreadDetailsResponse().MapToReponse(subThread));
            }

            return subThreadDetailsResponses;
        }
    }
}
