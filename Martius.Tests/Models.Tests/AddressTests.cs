using System;
using System.Reflection;
using FluentAssertions;
using Martius.Domain;
using Martius.Infrastructure;
using NUnit.Framework;

namespace Martius.Tests.Models.Tests
{
    [TestFixture]
    public class AddressTests
    {
        private Address _defaultAddress;
        private Address _anotherAddress;
        private FieldInfo _buildingNumberInfo;
        private FieldInfo _buildingExtraInfo;

        [SetUp]
        public void SetUp()
        {
            _defaultAddress = new Address("city", "street", 1, 1);
            _anotherAddress = new Address("city", "street", 2, 2);

            var defaultBuildingType = _defaultAddress.GetType();
            _buildingNumberInfo =
                defaultBuildingType.GetField("_buildingNumber", BindingFlags.NonPublic | BindingFlags.Instance);
            _buildingExtraInfo =
                defaultBuildingType.GetField("_buildingExtra", BindingFlags.NonPublic | BindingFlags.Instance);
        }

        [Test]
        public void Should_GenerateSameHashCode_ForSameObject()
        {
            var defaultAddressCopy = ObjectCopier.DeepCopy(_defaultAddress);

            var defaultHashCode = _defaultAddress.GetHashCode();
            var copyHashCode = defaultAddressCopy.GetHashCode();

            defaultHashCode.Should().Be(copyHashCode);
        }

        [Test]
        public void Should_GenerateDifferentHashCodes_ForDifferentObjects()
        {
            var defaultHashCode = _defaultAddress.GetHashCode();
            var anotherHashCode = _anotherAddress.GetHashCode();

            defaultHashCode.Should().NotBe(anotherHashCode);
        }

        [Test]
        public void Should_BeEqualToItself()
        {
            _defaultAddress.Should().Be(_defaultAddress);
        }

        [Test]
        public void Should_NotBeEqualToAnotherAddress()
        {
            var result = _defaultAddress.Equals(_anotherAddress);
            result.Should().BeFalse();
        }

        [Test]
        public void Should_NotBeEqualToAnotherType()
        {
            // ReSharper disable once SuspiciousTypeConversion.Global
            var result = _defaultAddress.Equals("address");
            result.Should().BeFalse();
        }

        [Test]
        public void Should_NotBeEqualToNull()
        {
            var result = _defaultAddress.Equals(null);
            result.Should().BeFalse();
        }

        [Test]
        public void Should_AlphabeticallyCompareToOtherAddress()
        {
            var lessComparisonResult = _defaultAddress.CompareTo(_anotherAddress);
            var sameComparisonResult = _defaultAddress.CompareTo(_defaultAddress);
            var greaterComparisonResult = _defaultAddress.CompareTo(new Address("city", "street", 0, 0));

            lessComparisonResult.Should().BeLessThan(0);
            sameComparisonResult.Should().Be(0);
            greaterComparisonResult.Should().BeGreaterThan(0);
        }

        [Test]
        public void Should_ReturnOne_WhenComparedToNull()
        {
            var comparisonResult = _defaultAddress.CompareTo(null);
            comparisonResult.Should().Be(1);
        }

        [Test]
        public void Should_ThrowArgumentException_WhenComparedToAnotherType()
        {
            _defaultAddress.Invoking(m => m.CompareTo("string"))
                .Should().Throw<ArgumentException>()
                .WithMessage("obj is not an Address");
        }

        [Test]
        public void Should_ProvideSqlRepresentation()
        {
            var buildNumber = _buildingNumberInfo.GetValue(_defaultAddress).ToString();
            var buildExtra = _buildingExtraInfo.GetValue(_defaultAddress)?.ToString();

            var representation = _defaultAddress.ToSqlString();
            representation.Should().Contain("N'", AtLeast.Twice())
                .And.ContainAll(
                    _defaultAddress.City,
                    _defaultAddress.Street,
                    _defaultAddress.Building,
                    buildNumber,
                    buildExtra,
                    _defaultAddress.ApartmentNumber.ToString());
        }
    }
}