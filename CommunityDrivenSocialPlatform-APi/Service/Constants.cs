namespace CommunityDrivenSocialPlatform_APi.Service
{
    public static class Constants
    {
        public const string VoteSuccess = "Vote placed";
        public const string NotTheAuthorOfPost = "Sorry, You are not the author of this post to perform this action!";
        public const string NotTheAuthorOfComment = "Sorry, You are not the author of this comment to perform this action!";
        public static string PostDeleted = "Post successfully deleted!";
        public static string CommentMadeSuccessfully = "Comment successfully created!";
        public static string CommentUpdatedSuccessfully = "Comment successfully updated!";
        public static string CommentDeletedSuccessfully = "Comment successfully deleted!";

        public static string SubthreadMadeSuccessfully = "Subthread successfully created!";
        public static string SubthreadUpdatedSuccessfully = "Subthread successfully updated!";
        public static string SubthreadDeletedSuccessfully = "Subthread successfully deleted!";

        public static string NonExistentComment = "Comment does not exist.";
        public static string NoAuthorization = "You don't have authorization to perform this action.";

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

        public static string NotAuthorizedToEditSubthread(string subthreadName)
        {
            return $"Sorry, you don't have the authorization to edit '{subthreadName}' subthread";
        }


        public static string AlreadyMemberOfSubthread(string subthreadName)
        {
            return $"You are already a member of '{subthreadName}' subthread!";
        }

        public static string JoinedNewSubthread(string subthreadName)
        {
            return $"You have joined '{subthreadName}' subthread!";
        }

        public static string LeftSubthread(string subthreadName)
        {
            return $"You are no longer a member of '{subthreadName}' subthread!";
        }


    }
}
