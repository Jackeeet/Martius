using System;
using FluentAssertions;
using Martius.Domain;
using Martius.Infrastructure;
using Martius.Infrastructure.Extensions;
using NUnit.Framework;

namespace Martius.Tests.Models.Tests
{
    [TestFixture]
    public class PersonTests
    {
        private Person _person;
        private Person _personWithoutPatronym;

        [SetUp]
        public void SetUp()
        {
            _person = new Person("surname", "name", "patronym", new DateTime(2000, 1, 1));
            _personWithoutPatronym = new Person("surname", "name", "", new DateTime(2000, 1, 1));
        }

        [Test]
        public void Should_ProvideSqlRepresentation()
        {
            var representation = _person.ToSqlString();
            var withoutPatronym = _personWithoutPatronym.ToSqlString();

            SqlRepresentation_Should_BeCorrect(representation, _person);
            SqlRepresentation_Should_BeCorrect(withoutPatronym, _personWithoutPatronym);
        }

        private void SqlRepresentation_Should_BeCorrect(string representation, Person person)
        {
            representation.Should().Contain("N'", AtLeast.Twice())
                .And.ContainAll(
                    person.Name,
                    person.Surname,
                    person.Patronym,
                    person.DateOfBirth.GetSqlRepresentation());
        }

        [Test]
        public void Should_GenerateSameHashCode_ForSameObject()
        {
            var personCopy = ObjectCopier.DeepCopy(_person);

            var defaultHashCode = _person.GetHashCode();
            var copyHashCode = personCopy.GetHashCode();

            defaultHashCode.Should().Be(copyHashCode);
        }

        [Test]
        public void Should_GenerateDifferentHashCodes_ForDifferentObjects()
        {
            var defaultHashCode = _person.GetHashCode();
            var anotherHashCode = _personWithoutPatronym.GetHashCode();

            defaultHashCode.Should().NotBe(anotherHashCode);
        }

        [Test]
        public void Should_BeEqualToItself()
        {
            _person.Should().Be(_person);
        }

        [Test]
        public void Should_NotBeEqualToAnotherPerson()
        {
            var result = _person.Equals(_personWithoutPatronym);
            result.Should().BeFalse();
        }

        [Test]
        public void Should_NotBeEqualToAnotherType()
        {
            // ReSharper disable once SuspiciousTypeConversion.Global
            var result = _person.Equals("person");
            result.Should().BeFalse();
        }

        [Test]
        public void Should_NotBeEqualToNull()
        {
            var result = _person.Equals(null);
            result.Should().BeFalse();
        }
    }
}