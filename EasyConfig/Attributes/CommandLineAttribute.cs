namespace EasyConfig.Attributes
{
    public class CommandLineAttribute : ConfigurationAttribute
    {
        public CommandLineAttribute(string key) : base(key, ConfigurationSources.CommandLine)
        {
        }
    }
}