namespace CDSP_API.misc
{
    public static class ApiConstant
    {
        public const string GenericError = "Sorry, Something went wrong. Please try again.";
        public static class Identity
        {
            public static string UsernameAreadyInUse = "Sorry, Username is already in use.";
            public static string EmailAddressAreadyInUse = "Sorry, Email Address is already in use.";

            public static string InvalidPassword = "Sorry, Password must be between '6' and '255' characters.";


            public static readonly string NonExistentIdentity = "Sorry, Username or password is incorrect.";
            public static readonly string SignupFailed = "Sorry, Something went wrong signing you up.";

        }

        public static class User
        {
            public static readonly string SuccefullyCreatedUser = "User was successfully created.";
            public static readonly string FailedToCreateUser = "Sorry, Something went creating user.";
            public static readonly string NonExistentUser = "Sorry, No user in that username exists.";
            public static readonly string FailedToUpdateUser = "Sorry, Something went wrong updating user.";
            public static readonly string SuccefullyDeletedUser = "User was successfully deleted.";
        }

        public static class SubThread
        {
            public static string SubThreadNameAreadyInUse = "Sorry, Subthread name is already in use.";
            public static readonly string NonExistentSubThread = "Sorry, No subthread in that name exists.";

            public static readonly string SuccefullyCreatedUser = "User was successfully created.";
            public static readonly string FailedToCreateUser = "Sorry, Something went creating user.";

            public static readonly string FailedToUpdateSubThread = "Sorry, Something went wrong updating subthread.";

            public static readonly string SuccefullyDeletedSubThread = "Subthread was successfully deleted.";
            public static readonly string FailedToDeletedSubThread = "Sorry, Something went wrong deleting subthread.";

            public static readonly string AlreadyAnMember = "You are already a member of this subthread.";
            public static readonly string NotAnMember = "You are not a member of this subthread.";
            public static readonly string JoinedSuccess = "You are now a member of this subthread!";
            public static readonly string LeftSuccess = "You are no longer a member of this subthread!";

        }

        public static class Post
        {
            public static readonly string PostDeleted = "Post was successfully deleted";
            public static readonly string NonExistenPost = "Sorry, No post in that name exists.";
        }



    }
}
