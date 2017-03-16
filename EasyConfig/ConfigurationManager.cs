using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using EasyConfig.Attributes;
using EasyConfig.Exceptions;

namespace EasyConfig
{
    public class ConfigurationManager<T> where T : new()
    {
        public static T Initialize(params string[] args)
        {
            var parameters = new T();

            if (args == null) throw new ArgumentNullException(nameof(args));

            var argsDict = GetArgsDict(args);

            foreach (var fieldInfo in typeof(T).GetFields(BindingFlags.Public | BindingFlags.Instance))
            {
                var defaultAttribute = fieldInfo.GetCustomAttribute<DefaultAttribute>();
                var required = fieldInfo.GetCustomAttribute<RequiredAttribute>() != null;
                var configurationAttribute = fieldInfo.GetCustomAttribute<ConfigurationAttribute>();
                var shouldHideInLog = fieldInfo.GetCustomAttribute<SensitiveInformationAttribute>() != null;

                string value;

                var got = TryGet(argsDict, configurationAttribute.Key, configurationAttribute.ConfigurationSources, out value);

                if (!got)
                {
                    if (defaultAttribute == null)
                    {
                        if (required)
                        {
                            throw new ConfigurationMissingException(configurationAttribute.Key, fieldInfo.FieldType, configurationAttribute.ConfigurationSources);
                        }
                        continue;
                    }

                    value = defaultAttribute.Default.ToString();
                }

                SetValue(fieldInfo, value, configurationAttribute.Key, shouldHideInLog, ref parameters);
            }

            return parameters;
        }

        private static bool TryGet(Dictionary<string, string> argsDict, string key, ConfigurationSources sources, out string value)
        {
            bool got = false;
            string val = string.Empty;

            if (sources.HasFlag(ConfigurationSources.CommandLine))
            {
                got = argsDict.TryGetValue(key, out val);
            }

            if (!got && sources.HasFlag(ConfigurationSources.Environment))
            {
                val = Environment.GetEnvironmentVariable(key);
                got = !string.IsNullOrWhiteSpace(val);
            }

            value = val;
            return got;
        }

        private static Dictionary<string, string> GetArgsDict(string[] args)
        {
            var split = args.Select(x => x.Split('='));
            var argsDict = new Dictionary<string, string>();

            foreach (var pair in split)
            {
                argsDict[pair[0]] = pair[1];
            }

            return argsDict;
        }

        private static void SetValue(FieldInfo field, string value, string key, bool shouldHideInLog, ref T result)
        {
            if (field.FieldType == typeof(Uri))
            {
                Uri uri;

                if (!Uri.TryCreate(value, UriKind.Absolute, out uri))
                {
                    throw new ConfigurationTypeException(key, typeof(Uri));
                }

                LogConfigurationValue(key, value, shouldHideInLog);

                field.SetValue(result, new Uri(value));
            }
            else if (field.FieldType == typeof(int))
            {
                int i;
                if (!int.TryParse(value, out i))
                {
                    throw new ConfigurationTypeException(key, typeof(int));
                }

                LogConfigurationValue(key, value, shouldHideInLog);

                field.SetValue(result, i);
            }
            else
            {
                LogConfigurationValue(key, value, shouldHideInLog);

                field.SetValue(result, value);
            }
        }

        private static void LogConfigurationValue(string key, string value, bool shouldHideInLog)
        {
            if (shouldHideInLog)
            {
                value = new string('*', value.Length);
            }

            Console.WriteLine($"Using '{value}' for '{key}'");
        }
    }
}
