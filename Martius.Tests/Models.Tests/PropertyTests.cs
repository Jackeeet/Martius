using System.Globalization;
using FluentAssertions;
using Martius.Domain;
using Martius.Infrastructure;
using NUnit.Framework;
using static System.Decimal;

namespace Martius.Tests.Models.Tests
{
    [TestFixture]
    public class PropertyTests
    {
        private Address _defaultAddress;
        private Address _anotherAddress;
        private Property _defaultProperty;
        private Property _anotherProperty;

        [SetUp]
        public void SetUp()
        {
            _defaultAddress = new Address("city", "street", 1, 1);
            _anotherAddress = new Address("city", "street", 2, 2);
            _defaultProperty = new Property(1, _defaultAddress, 1, 30.0, false, false, false, Zero);
            _anotherProperty = new Property(2, _anotherAddress, 1, 30.0, false, false, false, Zero);
        }

        [Test]
        public void Should_GenerateSameHashCode_ForSameObject()
        {
            var defaultPropertyCopy = ObjectCopier.DeepCopy(_defaultProperty);

            var defaultHashCode = _defaultProperty.GetHashCode();
            var copyHashCode = defaultPropertyCopy.GetHashCode();

            defaultHashCode.Should().Be(copyHashCode);
        }

        [Test]
        public void Should_GenerateDifferentHashCodes_ForDifferentObjects()
        {
            var defaultHashCode = _defaultProperty.GetHashCode();
            var anotherHashCode = _anotherProperty.GetHashCode();

            defaultHashCode.Should().NotBe(anotherHashCode);
        }

        [Test]
        public void Should_BeEqualToItself()
        {
            _defaultProperty.Should().Be(_defaultProperty);
        }

        [Test]
        public void Should_NotBeEqualToAnotherProperty()
        {
            var result = _defaultProperty.Equals(_anotherProperty);
            result.Should().BeFalse();
        }

        [Test]
        public void Should_NotBeEqualToAnotherType()
        {
            // ReSharper disable once SuspiciousTypeConversion.Global
            var result = _defaultProperty.Equals("property");
            result.Should().BeFalse();
        }

        [Test]
        public void Should_NotBeEqualToNull()
        {
            var result = _defaultProperty.Equals(null);
            result.Should().BeFalse();
        }

        [Test]
        public void Should_ReturnAddressToString_WhenCallingToString()
        {
            var stringRepresentation = _defaultProperty.ToString();
            var addressRepresentation = _defaultAddress.ToString();

            stringRepresentation.Should().Be(addressRepresentation);
        }

        [Test]
        public void Should_ProvideSqlRepresentation()
        {
            var representation = _defaultProperty.ToSqlString();

            representation.Should().ContainAll(
                _defaultProperty.Address.ToSqlString(),
                _defaultProperty.RoomCount.ToString(),
                _defaultProperty.Area.ToString(CultureInfo.InvariantCulture),
                _defaultProperty.IsResidential ? "1" : "0",
                _defaultProperty.IsFurnished ? "1" : "0",
                _defaultProperty.HasParking ? "1" : "0",
                _defaultProperty.MonthlyPrice.ToString(CultureInfo.InvariantCulture));
        }
    }
}