using System;
using EasyConfig.Exceptions;
using NUnit.Framework;

namespace EasyConfig.UnitTests.Exceptions
{
    [TestFixture]
    class ConfigurationMissingExceptionTests
    {
        [Test]
        public void ctor_GivenNull_DoesntThrow()
        {
            Assert.DoesNotThrow(() => new ConfigurationMissingException(null, null, 0));
        }

        [Test]
        public void ctor_GivenConfigKey_IncludesInErrorMessage()
        {
            var key = Guid.NewGuid().ToString();
            var result = new ConfigurationMissingException(key, null, 0).Message;

            Assert.That(result, Does.Contain(key));
        }

        [Test]
        public void ctor_GivenType_IncludesInErrorMessage()
        {
            var type = GetType();
            var result = new ConfigurationMissingException(null, type, 0).Message;

            Assert.That(result, Does.Contain(type.ToString()));
        }

        [Test]
        public void ctor_GivenLocation_IncludesInErrorMessage()
        {
            var location = ConfigurationSources.CommandLine;
            var result = new ConfigurationMissingException(null, null, location).Message;

            Assert.That(result, Does.Contain(location.ToString()));
        }

        [Test]
        public void ctor_GivenMultipleLocations_IncludesAllInErrorMessage()
        {
            var location = ConfigurationSources.CommandLine | ConfigurationSources.Environment;
            var result = new ConfigurationMissingException(null, null, location).Message;

            Assert.That(result, Does.Contain(ConfigurationSources.CommandLine.ToString()));
            Assert.That(result, Does.Contain(ConfigurationSources.Environment.ToString()));
        }
    }
}
