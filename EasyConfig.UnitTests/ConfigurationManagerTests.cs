using System;
using EasyConfig.Attributes;
using EasyConfig.Exceptions;
using NUnit.Framework;
#pragma warning disable 649

namespace EasyConfig.UnitTests
{
    [TestFixture]
    public class ConfigurationManagerTests
    {
        [Test]
        public void Initialize_Object_GivenValidArgs_DoesntThrow()
        {
            Assert.DoesNotThrow(() => ConfigurationManager<object>.Initialize());
        }

        [Test]
        public void Initialize_UriRequired_GivenDgApiEndpoint_DoesntThrow()
        {
            Assert.DoesNotThrow(() => ConfigurationManager<UriRequired>.Initialize("endpoint=http://www.google.com"));
        }

        [Test]
        public void Initialize_UriRequired_GivenDgApiEndpoint_SetsApiEndpoint()
        {
            var parameters = ConfigurationManager<UriRequired>.Initialize("endpoint=http://www.google.com");

            Assert.That(parameters.Test, Is.Not.Null);
        }

        [Test]
        public void Initialize_UriRequired_GivenAUri_SetsUriCorrectly()
        {
            var endpoint = new Uri("http://www.google.com");
            var parameters = ConfigurationManager<UriRequired>.Initialize($"endpoint={endpoint}");

            Assert.That(parameters.Test, Is.EqualTo(endpoint));
        }

        [Test]
        public void Initialize_UriRequired_GivenNotAUri_Throws()
        {
            Assert.Throws<ConfigurationTypeException>(() => ConfigurationManager<UriRequired>.Initialize("endpoint=notanendpoint"));
        }

        [Test]
        public void Initialize_UriRequired_NotSupplied_IncludesBothLocationsInMessage()
        {
            ConfigurationMissingException exception = null;
            try
            {
                ConfigurationManager<UriRequired>.Initialize();
            }
            catch (ConfigurationMissingException e)
            {
                exception = e;
            }

            Assert.That(exception, Is.Not.Null);
            Assert.That(exception?.Message, Does.Contain("Environment"));
            Assert.That(exception?.Message, Does.Contain("CommandLine"));
        }

        [Test]
        public void Initialize_UriRequired_NotGivenRequiredParameter_Throws()
        {
            Assert.Throws<ConfigurationMissingException>(() => ConfigurationManager<UriRequired>.Initialize());
        }

        [Test]
        public void Initialize_InEnvironmentVariabless_RequiredParamInEnvironment_DoesntThrow()
        {
            Assert.DoesNotThrow(() => ConfigurationManager<InEnvironmentVariables>.Initialize());
        }

        [Test]
        public void Initialize_NotInEnvironmentVariables_RequiredParamNotInEnvironment_Throws()
        {
            Assert.Throws<ConfigurationMissingException>(() => ConfigurationManager<NotInEnvironmentVariables>.Initialize());
        }

        [Test]
        public void Initialize_NotInEnvironmentVariables_RequiredParamNotInEnvironmentButInArgs_Throws()
        {
            Assert.Throws<ConfigurationMissingException>(() => ConfigurationManager<NotInEnvironmentVariables>.Initialize("defninitely_not_in_environment_variables=shouldstillthrow"));
        }

        [Test]
        public void Initialize_NotRequired_ParamNotInEnvironmentButInArgs_DoesNotThrow()
        {
            Assert.DoesNotThrow(() => ConfigurationManager<NotRequired>.Initialize());
        }

        [Test]
        public void Initialize_HasDefault_UsesDefault()
        {
            var config = ConfigurationManager<HasDefault>.Initialize();

            Assert.That(config.Test, Is.EqualTo("defaulttest"));
        }

        [Test]
        public void Initialize_IntRequired_DoesNotThrow()
        {
            Assert.DoesNotThrow(() => ConfigurationManager<IntRequired>.Initialize("number=1"));
        }

        [Test]
        public void Initialize_IntRequired_SetsNumber()
        {
            var num = new Random().Next();
            var config = ConfigurationManager<IntRequired>.Initialize("number=" + num);

            Assert.That(config.Test, Is.EqualTo(num));
        }

        [Test]
        public void Initialize_IntRequired_GivenNotAnInt_Throws()
        {
            Assert.Throws<ConfigurationTypeException>(() => ConfigurationManager<IntRequired>.Initialize("number=notaninteger"));
        }

        private class UriRequired
        {
            [EnvironmentOrCommandLine("endpoint"), Required]
            public Uri Test;
        }

        private class IntRequired
        {
            [EnvironmentOrCommandLine("number"), Required]
            public int Test;
        }

        private class InEnvironmentVariables
        {
            [Environment("path"), Required]
            public string Test;
        }

        private class NotInEnvironmentVariables
        {
            [Environment("defninitely_not_in_environment_variables"), Required]
            public string Test;
        }

        private class NotRequired
        {
            [Environment("defninitely_not_in_environment_variables")]
            public string Test;
        }

        private class HasDefault
        {
            [Environment("defninitely_not_in_environment_variables"), Default("defaulttest")]
            public string Test;
        }
    }
}
