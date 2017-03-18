using EasyConfig.Attributes;
using System;

namespace EasyConfig.UnitTests
{

    internal class UriRequired
    {
        [EnvironmentOrCommandLine("endpoint"), Required]
        public Uri Test;
    }

    internal class IntRequired
    {
        [EnvironmentOrCommandLine("number"), Required]
        public int Test;
    }

    internal class InEnvironmentVariables
    {
        internal const string EnvironmentVariableTestKey = "EasyConfig_this_environment_variable_should_only_exist_during_these_tests";

        [Environment(EnvironmentVariableTestKey), Required]
        public string Test;
    }

    internal class NotInEnvironmentVariables
    {
        [Environment("shouldnt_be_in_environment_variables"), Required]
        public string Test;
    }

    internal class NotRequired
    {
        [Environment("shouldnt_be_in_environment_variables")]
        public string Test;
    }

    internal class HasDefault
    {
        [Environment("shouldnt_be_in_environment_variables"), Default("defaulttest")]
        public string Test;
    }

    internal class InAppConfig
    {
        [AppConfig("InAppConfig")]
        public string Test;
    }

    internal class NotInAppConfig
    {
        [AppConfig("ShouldNeverBeInAppConfig"), Required]
        public string Test;
    }

    internal class DefaultingAppConfig
    {
        [AppConfig("ShouldNeverBeInAppConfig"), Default("sample")]
        public string Test;
    }

    internal class HasProperty
    {
        [CommandLine("exists")]
        public string Test { get; set; }
    }

    internal class HasField
    {
        [CommandLine("exists")]
        public string Test;
    }

    internal class HasThreePropertiesAndThreeFields
    {
        public string Feild1;
        public string Field2;
        public string Field3;

        public string Prop1 { get; set; }
        public string Prop2 { get; set; }
        public string Prop3 { get; set; }
    }
}
