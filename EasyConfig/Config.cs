using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Reflection;
using EasyConfig.Attributes;
using EasyConfig.Exceptions;
using static EasyConfig.Reflection.MemberInfoReflector;

namespace EasyConfig
{
    public class Config
    {
        public static T Populate<T>(params string[] args) where T : new()
        {
            var parameters = new T();

            if (args == null) throw new ArgumentNullException(nameof(args));

            var argsDict = GetArgsDict(args);

            var members = GetFieldsAndProperties(typeof(T).GetMembers(BindingFlags.Public | BindingFlags.Instance));

            foreach (var member in members)
            {
                var defaultAttribute = member.GetCustomAttribute<DefaultAttribute>();
                var required = member.GetCustomAttribute<RequiredAttribute>() != null;
                var configurationAttribute = member.GetCustomAttribute<ConfigurationAttribute>();
                var shouldHideInLog = member.GetCustomAttribute<SensitiveInformationAttribute>() != null;

                string value;

                var got = TryGet(argsDict, configurationAttribute.Key, configurationAttribute.ConfigurationSources, out value);

                if (!got)
                {
                    if (defaultAttribute == null)
                    {
                        if (required)
                        {
                            throw new ConfigurationMissingException(configurationAttribute.Key, member.GetUnderlyingType(), configurationAttribute.ConfigurationSources);
                        }
                        continue;
                    }

                    value = defaultAttribute.Default.ToString();
                }

                Action<MemberInfo, T, object> setterAction = GetSetterAction<T>(member);

                SetValue(
                    member,
                    member.GetUnderlyingType(),
                    setterAction,
                    value,
                    configurationAttribute.Key,
                    shouldHideInLog,
                    ref parameters);
            }

            return parameters;
        }

        private static Action<MemberInfo, T, object> GetSetterAction<T>(MemberInfo member)
        {
            Action<MemberInfo, T, object> setterAction;
            if (member.MemberType == MemberTypes.Field)
            {
                setterAction = (memberInfo, result, toSet) => { (memberInfo as FieldInfo).SetValue(result, toSet); };
            }

            else if (member.MemberType == MemberTypes.Property)
            {
                setterAction = (memberInfo, result, toSet) => { (memberInfo as PropertyInfo).SetValue(result, toSet); };
            }

            else
            {
                throw new ArgumentException("Input MemberInfo must be of type FieldInfo or PropertyInfo");
            }

            return setterAction;
        }

        private static bool TryGet(Dictionary<string, string> argsDict, string key, ConfigurationSources sources, out string value)
        {
            bool got = false;
            string val = string.Empty;

            if (sources.HasFlag(ConfigurationSources.CommandLine))
            {
                got = argsDict.TryGetValue(key, out val);
            }

            if (!got && sources.HasFlag(ConfigurationSources.AppConfig))
            {
                val = ConfigurationManager.AppSettings[key];
                if (!string.IsNullOrWhiteSpace(val))
                {
                    got = true;
                }
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

        private static void SetValue<T>(MemberInfo member, Type memberType, Action<MemberInfo, T, object> SetValue, string value, string key, bool shouldHideInLog, ref T result) where T : new()
        {
            if (memberType == typeof(Uri))
            {
                Uri uri;

                if (!Uri.TryCreate(value, UriKind.Absolute, out uri))
                {
                    throw new ConfigurationTypeException(key, typeof(Uri));
                }

                LogConfigurationValue(key, value, shouldHideInLog);

                SetValue(member, result, uri);
            }
            else if (memberType == typeof(int))
            {
                int i;
                if (!int.TryParse(value, out i))
                {
                    throw new ConfigurationTypeException(key, typeof(int));
                }

                LogConfigurationValue(key, value, shouldHideInLog);

                SetValue(member, result, i);
            }
            else
            {
                LogConfigurationValue(key, value, shouldHideInLog);

                SetValue(member, result, value);
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
