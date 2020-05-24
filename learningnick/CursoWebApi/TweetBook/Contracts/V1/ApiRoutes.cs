namespace TweetBook.Contracts.V1
{
    public static class ApiRoutes
    {
        public const string Root = "api";
        public const string Version = "v1";

        public const string Base = Root + "/" + Version;
        public static class Posts
        {
            public const  string GetAll = Base + "/Posts";
            public const  string Get = Base + "/Posts/{Id}";
            public const  string Update = Base + "/Posts/{Id}";
            public const  string Delete = Base + "/Posts/{Id}";
            public const  string Create = Base + "/Posts";

       }

        public static class Identity
       {
           public const string Register = Base + "/Register";
       }

      
    }
}