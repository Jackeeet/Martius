using System;
using FluentAssertions;
using Martius.Infrastructure.Extensions;
using NUnit.Framework;

namespace Martius.Tests.Infrastructure.Tests
{
    [TestFixture]
    public class DateTimeExtensionsTests
    {
        [Test]
        public void Should_CalculateMonthsCorrectly_WithFullMonths()
        {
            var jan1 = new DateTime(2000, 1, 1);
            var feb1 = new DateTime(2000, 2, 1);
            var aug1 = new DateTime(2000, 8, 1);

            var janToFeb = jan1.CalculateMonthsUntil(feb1);
            var janToAug = jan1.CalculateMonthsUntil(aug1);

            janToFeb.Should().Be(1);
            janToAug.Should().Be(7);
        }
        
        [Test]
        public void Should_CalculateMonthsCorrectly_WithIncompleteMonths()
        {
            var jan1 = new DateTime(2000, 1, 1);
            var jan15 = new DateTime(2000, 1, 15);
            var feb1 = new DateTime(2000, 2, 1);

            var jan1ToJan15 = jan1.CalculateMonthsUntil(jan15);
            var jan15ToFeb1 = jan15.CalculateMonthsUntil(feb1);

            jan1ToJan15.Should().Be(0);
            jan15ToFeb1.Should().Be(1);
        }
        
        [Test]
        public void Should_CalculateMonthsCorrectly_WithLeapMonth()
        {
            var feb29 = new DateTime(2000, 2, 29);
            var mar28 = new DateTime(2000, 3, 28);
            var mar29 = new DateTime(2000, 3, 29);

            var feb29ToMar28 = feb29.CalculateMonthsUntil(mar28);
            var feb29ToMar29 = feb29.CalculateMonthsUntil(mar29);

            feb29ToMar28.Should().Be(1);
            feb29ToMar29.Should().Be(1);
        }
        
        [Test]
        public void Should_CalculateMonthsCorrectly_WithDifferentYears()
        {
            var firstYearJan1 = new DateTime(2000, 1, 1);
            var secondYearFeb1 = new DateTime(2001, 2, 1);

            var months = firstYearJan1.CalculateMonthsUntil(secondYearFeb1);

            months.Should().Be(13);
        }
        
        [Test]
        public void Should_CalculateMonthsCorrectly_WhenDatesAreSwapped()
        {
            var startDate = new DateTime(2000, 1, 1);
            var endDate = new DateTime(2000, 8, 1);

            var months = endDate.CalculateMonthsUntil(startDate);
            
            months.Should().Be(7);
        }

        [Test]
        public void Should_GetCorrectSqlRepresentation()
        {
            var date = new DateTime(1900, 1, 1);
            var representation = date.GetSqlRepresentation();

            representation.Should().Be("'1900-01-01'");
        }
    }
}