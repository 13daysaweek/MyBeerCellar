namespace MyBeerCellar.API
{
    public static class Constants
    {
        public static class ConfigurationKeys
        {
            public const string MyBeerCellarDbConnectionString = "ConnectionStrings:MyBeerCellarDb";
            public const string AppConfigConnectionStringKey = "ConnectionStrings:AppConfig";
            public const string AppInsightsInstrumentationKeyKey = "ApplicationInsights:InstrumentationKey";
        }

        public static class DataConfiguration
        {
            public const string SchemaName = "dbo";
            public const string CurrentUtcDateTimeDefault = "GETUTCDATE()";
        }

        public static class MigrationUtility
        {
            public const string LaunchPrefix = "run-migration";
            public const string ConnectionStringArg = "--connection-string";
        }
    }
}