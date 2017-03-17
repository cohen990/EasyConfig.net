namespace EasyConfig.Attributes
{
    public class AppConfigAttribute : ConfigurationAttribute
    {
        public AppConfigAttribute(string key) : base(key, ConfigurationSources.AppConfig)
        {
        }
    }
}