using CDSP_API.Models;

namespace CDSP_API.Contracts.V1.Requests
{
    public class UpdateSubThreadRequest
    {
        public string Description { get; set; }
        public string WelcomeMessage { get; set; }

        public SubThread MapToModel(SubThread subThread)
        {
            if (Description != null)
                subThread.Description = Description;
            if (WelcomeMessage != null)
                subThread.WelcomeMessage = WelcomeMessage;
            return subThread;
        }
    }
}
