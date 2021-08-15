using FluentAssertions;
using Martius.Infrastructure.Extensions;
using NUnit.Framework;

namespace Martius.Tests.Infrastructure.Tests
{
    [TestFixture]
    public class StringExtensionsTests
    {
        [Test]
        public void Should_ConvertIntegerStringToNullableInt()
        {
            var integerString = "12";
            var nullable = integerString.ToNullableInt();

            nullable.Should().NotBeNull().And.Be(12);
        }
        
        [Test]
        public void Should_ConvertNonIntegerStringToNull()
        {
            var nonIntegerString = "129kjf482";
            var nullable = nonIntegerString.ToNullableInt();

            nullable.Should().BeNull();
        }
        
        [Test]
        public void Should_ConvertEmptyStringToNull()
        {
            var emptyString = string.Empty;
            var nullable = emptyString.ToNullableInt();

            nullable.Should().BeNull();
        }
    }
}