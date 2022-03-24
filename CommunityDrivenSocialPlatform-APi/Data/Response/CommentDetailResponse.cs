using System;

namespace CommunityDrivenSocialPlatform_APi.Model.Response
{
    public class CommentDetailResponse
    {
        public int CommentId { get; set; }
        public string Body { get; set; }
        public string Author { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
