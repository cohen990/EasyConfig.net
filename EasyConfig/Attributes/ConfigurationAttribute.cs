using System;

namespace EasyConfig.Attributes
{
    public class ConfigurationAttribute : Attribute
    {
        public string Key { get; set; }

        public ConfigurationSources ConfigurationSources;

        public ConfigurationAttribute(string key, ConfigurationSources configurationSources)
        {
            ConfigurationSources = configurationSources;
            Key = key;
        }

    }
}