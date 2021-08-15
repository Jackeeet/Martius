using System;
using FluentAssertions;
using Martius.Domain;
using Martius.Infrastructure;
using NUnit.Framework;

namespace Martius.Tests.Models.Tests
{
    [TestFixture]
    public class TenantTests
    {
        private Person _person;
        private Person _anotherPerson;
        private Tenant _defaultTenant;
        private Tenant _anotherTenant;

        [SetUp]
        public void SetUp()
        {
            _person = new Person("surname", "name", "patronym", new DateTime(2000, 1, 1));
            _anotherPerson = new Person("surname", "name", "", new DateTime(2000, 1, 1));
            _defaultTenant = new Tenant(1, _person, "88005553535", "0000000000");
            _anotherTenant = new Tenant(2, _anotherPerson, "88008880000", "0000111111");
        }

        [Test]
        public void Should_GenerateSameHashCode_ForSameObject()
        {
            var defaultPersonCopy = ObjectCopier.DeepCopy(_defaultTenant);

            var defaultHashCode = _defaultTenant.GetHashCode();
            var copyHashCode = defaultPersonCopy.GetHashCode();

            defaultHashCode.Should().Be(copyHashCode);
        }

        [Test]
        public void Should_GenerateDifferentHashCodes_ForDifferentObjects()
        {
            var defaultHashCode = _defaultTenant.GetHashCode();
            var anotherHashCode = _anotherTenant.GetHashCode();

            defaultHashCode.Should().NotBe(anotherHashCode);
        }

        [Test]
        public void Should_BeEqualToItself()
        {
            _defaultTenant.Should().Be(_defaultTenant);
        }

        [Test]
        public void Should_NotBeEqualToAnotherTenant()
        {
            var result = _defaultTenant.Equals(_anotherTenant);
            result.Should().BeFalse();
        }

        [Test]
        public void Should_NotBeEqualToAnotherType()
        {
            // ReSharper disable once SuspiciousTypeConversion.Global
            var result = _defaultTenant.Equals("lease");
            result.Should().BeFalse();
        }

        [Test]
        public void Should_NotBeEqualToNull()
        {
            var result = _defaultTenant.Equals(null);
            result.Should().BeFalse();
        }

        [Test]
        public void Should_ProvideStringRepresentation()
        {
            var representation = _defaultTenant.ToString();
            representation.Should().ContainAll(_defaultTenant.PersonInfo.ToString(), _defaultTenant.PassportNumber);
        }
        
        [Test]
        public void Should_ProvideSqlRepresentation()
        {
            var representation = _defaultTenant.ToSqlString();
            
            representation.Should().ContainAll(
                _defaultTenant.PersonInfo.ToSqlString(),
                _defaultTenant.PhoneNumber,
                _defaultTenant.PassportNumber);
        }
    }
}