using EasyConfig.Reflection;
using NUnit.Framework;
using System;
using System.Linq;
using System.Reflection;

namespace EasyConfig.UnitTests.Reflection
{
    [TestFixture]
    public class MemberInfoReflectorTests
    {
        [Test]
        public void GetUnderlyingType_GivenNull_Throws()
        {
            MemberInfo member = null;

            Assert.Throws<ArgumentNullException>(() => member.GetUnderlyingType());
        }
        [Test]
        public void GetUnderlyingType_GivenFieldInfo_DoesntThrow()
        {
            var member = typeof(HasField).GetFields().Single() as MemberInfo;

            Assert.DoesNotThrow(() => member.GetUnderlyingType());
        }

        [Test]
        public void GetUnderlyingType_GivenFieldInfo_ReturnsCorrectType()
        {
            var member = typeof(HasField).GetFields().Single() as MemberInfo;

            var result = member.GetUnderlyingType();

            Assert.That(result, Is.EqualTo(typeof(string)));
        }

        [Test]
        public void GetUnderlyingType_GivenPropertyInfo_DoesntThrow()
        {
            var member = typeof(HasProperty).GetProperties().Single() as MemberInfo;

            Assert.DoesNotThrow(() => member.GetUnderlyingType());
        }

        [Test]
        public void GetUnderlyingType_GivenPropertyInfo_ReturnsCorrectType()
        {
            var member = typeof(HasProperty).GetProperties().Single() as MemberInfo;

            var result = member.GetUnderlyingType();

            Assert.That(result, Is.EqualTo(typeof(string)));
        }

        [Test]
        public void GetFieldsAndProperties_GivenNull_ReturnsEmptyList()
        {
            var result = MemberInfoReflector.GetFieldsAndProperties(null);

            Assert.That(result, Is.Empty);
        }

        [Test]
        public void GetFieldsAndProperties_GivenMembersFromHasField_Returns1Member()
        {
            var members = typeof(HasField).GetMembers();
            var result = MemberInfoReflector.GetFieldsAndProperties(members).Count();

            Assert.That(result, Is.EqualTo(1));
        }

        [Test]
        public void GetFieldsAndProperties_GivenMembersFromHasProperty_Returns1Member()
        {
            var members = typeof(HasProperty).GetMembers();
            var result = MemberInfoReflector.GetFieldsAndProperties(members).Count();

            Assert.That(result, Is.EqualTo(1));
        }

        [Test]
        public void GetFieldsAndProperties_GivenMembersFromHasThreePropertiesAndThreeFields_Returns6Members()
        {
            var members = typeof(HasThreePropertiesAndThreeFields).GetMembers();
            var result = MemberInfoReflector.GetFieldsAndProperties(members).Count();

            Assert.That(result, Is.EqualTo(6));
        }
    }
}
