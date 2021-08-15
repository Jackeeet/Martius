using System;
using Martius.Infrastructure;
using Martius.Infrastructure.Extensions;

namespace Martius.Domain
{
    [Serializable]
    public class Person
    {
        public readonly string Surname;
        public readonly string Name;
        public readonly string Patronym;
        public readonly DateTime DateOfBirth;

        public Person(string surname, string name, string patronym, DateTime dateOfBirth)
        {
            Surname = surname;
            Name = name;
            Patronym = patronym;
            DateOfBirth = dateOfBirth;
        }

        public string ToSqlString()
        {
            return $"N'{Surname}', N'{Name}', N'{Patronym}', {DateOfBirth.GetSqlRepresentation()}";
        }

        public override string ToString()
        {
            var result = $"{Surname} {Name}";
            if (Patronym != null) result += $" {Patronym}";
            return result;
        }

        protected bool Equals(Person other)
        {
            return string.Equals(Surname, other.Surname, StringComparison.InvariantCultureIgnoreCase) &&
                   string.Equals(Name, other.Name, StringComparison.InvariantCultureIgnoreCase) &&
                   string.Equals(Patronym, other.Patronym, StringComparison.InvariantCultureIgnoreCase) &&
                   DateOfBirth.Equals(other.DateOfBirth);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((Person) obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                var hashCode = StringComparer.InvariantCultureIgnoreCase.GetHashCode(Surname);
                hashCode = (hashCode * 397) ^ StringComparer.InvariantCultureIgnoreCase.GetHashCode(Name);
                hashCode = (hashCode * 397) ^ StringComparer.InvariantCultureIgnoreCase.GetHashCode(Patronym);
                hashCode = (hashCode * 397) ^ DateOfBirth.GetHashCode();
                return hashCode;
            }
        }
    }
}