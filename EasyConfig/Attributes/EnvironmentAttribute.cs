namespace EasyConfig.Attributes
{
    public class EnvironmentAttribute : ConfigurationAttribute
    {
        public EnvironmentAttribute(string key) : base(key, ConfigurationSources.Environment)
        {
        }
    }
}