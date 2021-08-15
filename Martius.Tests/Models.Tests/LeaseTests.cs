using System;
using System.Globalization;
using FluentAssertions;
using Martius.Domain;
using Martius.Infrastructure;
using Martius.Infrastructure.Extensions;
using NUnit.Framework;

namespace Martius.Tests.Models.Tests
{
    [TestFixture]
    public class LeaseTests
    {
        private Address _address;
        private Property _property;
        private Person _person;
        private Tenant _tenant;
        private Lease _defaultLease;
        private Lease _anotherLease;

        [SetUp]
        public void SetUp()
        {
            _address = new Address("city", "street", 1, 1);
            _property = new Property(1, _address, 1,
                30.0, false, false, false, decimal.Zero);
            _person = new Person("surname", "name", "patronym",
                new DateTime(2000, 1, 1));
            _tenant = new Tenant(1, _person, "88005553535", "0000000000");
            _defaultLease = new Lease(1, _property, _tenant, decimal.One,
                new DateTime(2000, 1, 1), new DateTime(2000, 2, 1));
            _anotherLease = new Lease(2, _property, _tenant, decimal.One,
                new DateTime(2001, 1, 1), new DateTime(2001, 3, 5));
        }

        [Test]
        public void Should_GenerateSameHashCode_ForSameObject()
        {
            var defaultLeaseCopy = ObjectCopier.DeepCopy(_defaultLease);

            var defaultHashCode = _defaultLease.GetHashCode();
            var copyHashCode = defaultLeaseCopy.GetHashCode();

            defaultHashCode.Should().Be(copyHashCode);
        }

        [Test]
        public void Should_GenerateDifferentHashCodes_ForDifferentObjects()
        {
            var defaultHashCode = _defaultLease.GetHashCode();
            var anotherHashCode = _anotherLease.GetHashCode();

            defaultHashCode.Should().NotBe(anotherHashCode);
        }

        [Test]
        public void Should_BeEqualToItself()
        {
            _defaultLease.Should().Be(_defaultLease);
        }

        [Test]
        public void Should_NotBeEqualToAnotherLease()
        {
            var result = _defaultLease.Equals(_anotherLease);
            result.Should().BeFalse();
        }

        [Test]
        public void Should_NotBeEqualToAnotherType()
        {
            // ReSharper disable once SuspiciousTypeConversion.Global
            var result = _defaultLease.Equals("lease");
            result.Should().BeFalse();
        }

        [Test]
        public void Should_NotBeEqualToNull()
        {
            var result = _defaultLease.Equals(null);
            result.Should().BeFalse();
        }

        [Test]
        public void Should_ReturnPropertyAddress()
        {
            var address = _defaultLease.Address;
            address.Should().BeSameAs(_address);
        }

        [Test]
        public void Should_ReturnTenantFullName()
        {
            var tenantName = _defaultLease.TenantName;
            tenantName.Should().Be(_tenant.FullName);
        }

        [Test]
        public void Should_CalculateFullPriceCorrectly()
        {
            var defaultPrice = _defaultLease.FullPrice;
            var anotherPrice = _anotherLease.FullPrice;

            defaultPrice.Should().Be(1);
            anotherPrice.Should().Be(2);
        }

        [Test]
        public void Should_CheckEqualityWithoutId()
        {
            var defaultContentsCopy = new Lease(0, _property, _tenant, decimal.One,
                new DateTime(2000, 1, 1), new DateTime(2000, 2, 1));

            var equalsItself = _defaultLease.EqualsWithoutId(_defaultLease);
            var equalsAnother = _defaultLease.EqualsWithoutId(_anotherLease);
            var equalsContentsCopy = _defaultLease.EqualsWithoutId(defaultContentsCopy);

            equalsItself.Should().BeTrue();
            equalsAnother.Should().BeFalse();
            equalsContentsCopy.Should().BeTrue();
        }

        [Test]
        public void Should_ProvideStringRepresentation()
        {
            var result = _defaultLease.ToString();

            result.Should().ContainAll(
                _defaultLease.Id.ToString(),
                _defaultLease.Property.ToString(),
                _defaultLease.Tenant.ToString(),
                _defaultLease.MonthlyPrice.ToString(CultureInfo.InvariantCulture),
                _defaultLease.StartDate.ToString(),
                _defaultLease.EndDate.ToString());
        }

        [Test]
        public void Should_ProvideSqlRepresentation()
        {
            var representation = _defaultLease.ToSqlString();

            representation.Should().ContainAll(
                _defaultLease.Property.Id.ToString(),
                _defaultLease.Tenant.Id.ToString(),
                _defaultLease.FullPrice.ToString(CultureInfo.InvariantCulture),
                _defaultLease.StartDate.GetSqlRepresentation(),
                _defaultLease.EndDate.GetSqlRepresentation());
        }
    }
}