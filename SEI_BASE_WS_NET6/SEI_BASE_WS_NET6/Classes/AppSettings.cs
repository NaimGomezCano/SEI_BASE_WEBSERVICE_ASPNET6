namespace SEI_WEBSERVICE
{
    public static class AppSettings
    {
        public static string authToken;

        public static string DataBaseServer;
        public static string DataBaseName;
        public static string DataBaseUserName;
        public static string DataBasePassword;

        public static string companyDB { get; set; }
        public static string password { get; set; }
        public static string userName { get; set; }
        public static string language { get; set; }
        public static string uri { get; set; }

        public static void loadSettings(IConfiguration configuration)
        {
            authToken = configuration.GetSection("Settings").GetSection("Authentication")["token"];

            var slSection = configuration.GetSection("ServiceLayer");
            companyDB = slSection["companyDB"];
            password = slSection["password"];
            userName = slSection["userName"];
            language = slSection["language"];
            uri = slSection["url"];


            var sqlSection = configuration.GetSection("SQL");
            DataBaseServer = sqlSection["DataBaseServer"];
            DataBaseName = sqlSection["DataBaseName"];
            DataBaseUserName = sqlSection["DataBaseUserName"];
            DataBasePassword = sqlSection["DataBasePassword"];
        }
    }
}
