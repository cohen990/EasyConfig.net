using System;
using EasyConfig.Exceptions;
using NUnit.Framework;
#pragma warning disable 649

namespace EasyConfig.UnitTests
{
    [TestFixture]
    public class ConfigTests
    {
        [SetUp]
        public void SetUp()
        {
            Environment.SetEnvironmentVariable(InEnvironmentVariables.EnvironmentVariableTestKey, Guid.NewGuid().ToString(), EnvironmentVariableTarget.Process);
        }

        [TearDown]
        public void TearDown()
        {
            Environment.SetEnvironmentVariable(InEnvironmentVariables.EnvironmentVariableTestKey, null, EnvironmentVariableTarget.Process);
        }

        [Test]
        public void Populate_Object_GivenValidArgs_DoesntThrow()
        {
            Assert.DoesNotThrow(() => Config.Populate<object>());
        }

        [Test]
        public void Populate_UriRequired_GivenDgApiEndpoint_DoesntThrow()
        {
            Assert.DoesNotThrow(() => Config.Populate<UriRequired>("endpoint=http://www.google.com"));
        }

        [Test]
        public void Populate_UriRequired_GivenDgApiEndpoint_SetsApiEndpoint()
        {
            var parameters = Config.Populate<UriRequired>("endpoint=http://www.google.com");

            Assert.That(parameters.Test, Is.Not.Null);
        }

        [Test]
        public void Populate_UriRequired_GivenAUri_SetsUriCorrectly()
        {
            var endpoint = new Uri("http://www.google.com");
            var parameters = Config.Populate<UriRequired>($"endpoint={endpoint}");

            Assert.That(parameters.Test, Is.EqualTo(endpoint));
        }

        [Test]
        public void Populate_UriRequired_GivenNotAUri_Throws()
        {
            Assert.Throws<ConfigurationTypeException>(() => Config.Populate<UriRequired>("endpoint=notanendpoint"));
        }

        [Test]
        public void Populate_UriRequired_NotGivenRequiredParameter_Throws()
        {
            Assert.Throws<ConfigurationMissingException>(() => Config.Populate<UriRequired>());
        }

        [Test]
        public void Populate_InEnvironmentVariabless_RequiredParamInEnvironment_DoesntThrow()
        {
            Assert.DoesNotThrow(() => Config.Populate<InEnvironmentVariables>());
        }

        [Test]
        public void Populate_NotInEnvironmentVariables_RequiredParamNotInEnvironment_Throws()
        {
            Assert.Throws<ConfigurationMissingException>(() => Config.Populate<NotInEnvironmentVariables>());
        }

        [Test]
        public void Populate_NotInEnvironmentVariables_RequiredParamNotInEnvironmentButInArgs_Throws()
        {
            Assert.Throws<ConfigurationMissingException>(() => Config.Populate<NotInEnvironmentVariables>("defninitely_not_in_environment_variables=shouldstillthrow"));
        }

        [Test]
        public void Populate_NotRequired_ParamNotInEnvironmentButInArgs_DoesNotThrow()
        {
            Assert.DoesNotThrow(() => Config.Populate<NotRequired>());
        }

        [Test]
        public void Populate_HasDefault_UsesDefault()
        {
            var config = Config.Populate<HasDefault>();

            Assert.That(config.Test, Is.EqualTo("defaulttest"));
        }

        [Test]
        public void Populate_IntRequired_DoesNotThrow()
        {
            Assert.DoesNotThrow(() => Config.Populate<IntRequired>("number=1"));
        }

        [Test]
        public void Populate_IntRequired_SetsNumber()
        {
            var num = new Random().Next();
            var config = Config.Populate<IntRequired>("number=" + num);

            Assert.That(config.Test, Is.EqualTo(num));
        }

        [Test]
        public void Populate_IntRequired_GivenNotAnInt_Throws()
        {
            Assert.Throws<ConfigurationTypeException>(() => Config.Populate<IntRequired>("number=notaninteger"));
        }

        [Test]
        public void Populate_InAppConfig_SetsValue()
        {
            var result = Config.Populate<InAppConfig>().Test;

            Assert.That(result, Is.EqualTo("testvalue"));
        }

        [Test]
        public void Populate_NotInAppConfig_Required_Throws()
        {
            Assert.Throws<ConfigurationMissingException>(() => Config.Populate<NotInAppConfig>());
        }

        [Test]
        public void Populate_DefaultingAppConfig_SetsAsDefault()
        {
            var result = Config.Populate<DefaultingAppConfig>().Test;
            Assert.That(result, Is.EqualTo("sample"));
        }

        [Test]
        public void Populate_AsProperty_PublicGetterSetter_CanSet()
        {
            var result = Config.Populate<HasProperty>("exists=thing").Test;
            Assert.That(result, Is.EqualTo("thing"));
        }

        [Test]
        public void Populate_HasBool_DoesNotThrow()
        {
            Assert.DoesNotThrow(() => Config.Populate<HasBool>("exists=true"));
        }

        [Test]
        public void Populate_HasBool_SetsBool()
        {
            var result = Config.Populate<HasBool>("exists=true").Test;

            Assert.That(result, Is.True);
        }

        [Test]
        public void Populate_HasDouble_SetsDouble()
        {
            double val = 1.0527498;
            var result = Config.Populate<HasDouble>("exists=" + val).Test;

            Assert.That(result, Is.EqualTo(val));
        }
        
        [Test]
        public void Populate_GivenUnsupportedType_Throws()
        {
            Assert.Throws<TypeNotSupportedException>(() => Config.Populate<HasUnsupportedType>("exists={Prop:'value'}"));
        }
    }
}
