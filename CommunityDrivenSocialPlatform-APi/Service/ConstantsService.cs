namespace CommunityDrivenSocialPlatform_APi.Service
{
    public static class ConstantsService
    {
        public const string VoteSuccess = "Vote placed";
        public const string NotTheAuthorOfPost = "Sorry, You are not the author of this post to perform this action!";
        public static string PostDeleted = "Post successfully deleted!";
        public static string NonExistentSubThread(string name)
        {
            return $"Sorry, no subthread with the name '{name}' exisits.";
        }

        public static string NonExistentPost(string name)
        {
            return $"Sorry, no post with the name '{name}' exisits.";
        }

        public static string NonExistentPost(int id)
        {
            return $"Sorry, no post with the id '{id}' exisits.";
        }

        public static string NonExistentPost(int postId, string subthreadName)
        {
            return $"Sorry, no post with the id '{postId}' on '{subthreadName}' subthread exisits.";
        }

        public static string NonExistentPost(string title, string subthreadName)
        {
            return $"Sorry, no post with the name '{title}' on '{subthreadName}' subthread exisits.";
        }

        public static string NotMemberOfSubthread(string subthreadName)
        {
            return $"Sorry, you must to be a member of '{subthreadName}' subthread to perform this action.";
        }

    }
}
