using CommunityDrivenSocialPlatform_APi.Model.Response;
using System.Collections.Generic;

namespace CommunityDrivenSocialPlatform_APi.Model.Request
{
    public class PostDetailsResponse
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Body { get; set; }
        public string Author { get; set; }
        public int Votes { get; set; }
        public List<CommentDetailResponse> Comments { get; set; }
    }
}
