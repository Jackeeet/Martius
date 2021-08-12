using FluentAssertions;
using Martius.Infrastructure.Extensions;
using NUnit.Framework;

namespace Martius.Tests.Infrastructure.Tests
{
    [TestFixture]
    public class DecimalExtensionsTests
    {
        [Test]
        public void Should_ReturnDecimalPoints()
        {
            var doubleDigitValue = new decimal(12.34);
            var roundDoubleDigitValue = new decimal(12.30);
            var singleDigitValue = new decimal(12.04);
            var zeroValue = new decimal(12.00);

            var doublePoints = doubleDigitValue.GetDecimalPoints();
            var roundDoublePoints = roundDoubleDigitValue.GetDecimalPoints();
            var singlePoint = singleDigitValue.GetDecimalPoints();
            var zeroPoints = zeroValue.GetDecimalPoints();

            doublePoints.Should().Be(new decimal(34));
            roundDoublePoints.Should().Be(new decimal(30));
            singlePoint.Should().Be(new decimal(4));
            zeroPoints.Should().Be(decimal.Zero);
        }
    }
}