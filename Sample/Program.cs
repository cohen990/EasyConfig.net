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
            /* Pass in command line arguments like the following:
                uri_required=https://www.endpoint1.com uri_not_required=https://www.endpoint2.com string_required_commandline=myusername string_sensitive_required=willbeobfuscatedinlogs int_default=12345 bool_example=true custom_type=val1:thingy,val2:otherthingy
            */
            Config.RegisterTypeSupport(typeof(CustomType), (input) =>
            {
                var result = new CustomType();
                var split = input.Split(',');
                result.Val1 = split[0].Split(':')[1];
                result.Val2 = split[1].Split(':')[1];

                return result;
            });

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

            [CommandLine("bool_example")]
            public bool BoolExample { get; set; }

            [CommandLine("custom_type")]
            public CustomType CustomTypeExample { get; set; }
        }

        public class CustomType
        {
            public string Val1;
            public string Val2;
        }
    }
}
