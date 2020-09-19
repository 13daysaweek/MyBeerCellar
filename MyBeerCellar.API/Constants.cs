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
            public const string NvarcharDataType = "nvarchar";
            public const string CurrentUtcDateTimeDefault = "GETUTCDATE()";
        }
    }
}