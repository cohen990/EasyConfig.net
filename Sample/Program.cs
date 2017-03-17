using System;
using EasyConfig.Attributes;

#pragma warning disable 649

namespace EasyConfig.Sample
{
    class Program
    {
        private static Settings _settings;

        static void Main(string[] args)
        {
            _settings = Config.Populate<Settings>(args);
        }

        public class Settings
        {
            // Can be supplied by environment or command line. If not supplied, throws a ConfigurationMissingException
            [EnvironmentOrCommandLine("uri_required"), Required]
            public Uri Endpoint1;

            [EnvironmentOrCommandLine("uri_not_required")]
            public Uri Endpoint2;

            [CommandLine("string_required_commandline"), Required]
            public string Username;

            [CommandLine("string_sensitive_required"), Required, SensitiveInformation]
            public string Password;

            [CommandLine("int_default"), Default(1000)]
            public int Defaultable;

            [AppConfig("AppConfigSetting")]
            public string InAppConfig;
        }
    }
}
