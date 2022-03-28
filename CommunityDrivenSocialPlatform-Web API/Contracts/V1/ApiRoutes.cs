namespace CDSP_API.Contracts
{
    public static class ApiRoutes
    {
        public const string BaseAndVersionV1 = "api/v1";
        public static class Controller
        {
            public const string IdentityController = "/identity";
            public const string UsersController = "/user";
            public const string SubThreadController = "/subthread";
            public const string PostController = "/post";
            public const string CommentController = "/comment";
            public const string FeedController = "/feed";

            public static class Action
            {
                public const string Signup = "signup";
                public const string Signin = "signin";
                public const string RefreshToken = "refreshtoken";
            }
            public static class RouteVariable
            {
                public const string Username = "{username}";
                public const string SubThreadName = "{name}";
                public const string SubThreadJoin = "{name}/join";
                public const string SubThreadLeave = "{name}/leave";
                public const string SubThreadUsers = "{name}/users";

            }

        }

    }
}
