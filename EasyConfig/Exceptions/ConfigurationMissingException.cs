using System;
using System.Collections.Generic;

namespace EasyConfig.Exceptions
{
    public class ConfigurationMissingException : Exception
    {
        public ConfigurationMissingException(string configKey, Type type, ConfigurationSources sources)
            : base(GetMessage(configKey, type, sources))
        {
        }
        public static string GetMessage(string configKey, Type type, ConfigurationSources sources) {

            var message =
                $"Configuration for key '{configKey}' was not found. Please supply a {type} at one of the following sources:";

            foreach (var configurationLocation in GetLocations(sources))
            {
                message += $"\n{configurationLocation}";
            }

            return message;
        }

        static IEnumerable<ConfigurationSources> GetLocations(ConfigurationSources input)
        {
            foreach (ConfigurationSources value in Enum.GetValues(input.GetType()))
                if (input.HasFlag(value))
                    yield return value;
        }
    }
}