using System;
using FluentAssertions;
using Martius.Infrastructure;
using NUnit.Framework;

namespace Martius.Tests.Infrastructure.Tests
{
    [TestFixture]
    public class ObjectCopierTests
    {
        [Test]
        public void Should_MakeDeepCopy()
        {
            var origin = new ClassA(1, new ClassB("test"));

            var copy = ObjectCopier.DeepCopy(origin);

            copy.Should().Be(origin);
            copy.Should().NotBeSameAs(origin);
        }
    }

    [Serializable]
    internal class ClassA
    {
        public ClassA(int valueProperty, ClassB referenceProperty)
        {
            ValueProperty = valueProperty;
            ReferenceProperty = referenceProperty;
        }

        protected bool Equals(ClassA other)
        {
            return ValueProperty == other.ValueProperty && Equals(ReferenceProperty, other.ReferenceProperty);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((ClassA) obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return (ValueProperty * 397) ^ (ReferenceProperty != null ? ReferenceProperty.GetHashCode() : 0);
            }
        }

        public int ValueProperty { get; }

        public ClassB ReferenceProperty { get; }
    }

    [Serializable]
    internal class ClassB
    {
        public ClassB(string name)
        {
            Name = name;
        }

        public string Name { get; }

        protected bool Equals(ClassB other)
        {
            return Name == other.Name;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((ClassB) obj);
        }

        public override int GetHashCode()
        {
            return (Name != null ? Name.GetHashCode() : 0);
        }
    }
}