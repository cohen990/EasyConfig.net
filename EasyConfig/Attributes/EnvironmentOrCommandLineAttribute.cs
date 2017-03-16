namespace EasyConfig.Attributes
{
    public class EnvironmentOrCommandLineAttribute : ConfigurationAttribute
    {
        public EnvironmentOrCommandLineAttribute(string key) : base(key, ConfigurationSources.CommandLine | ConfigurationSources.Environment)
        {
        }
    }
}