using System;

namespace Martius.Models
{
    public class Address
    {
        public readonly string City;
        public readonly string Street;
        public readonly string Building;
        public readonly int? ApartmentNumber;

        public Address(string city, string street, string building, int? apartmentNumber)
        {
            City = city;
            Street = street;
            Building = building;
            ApartmentNumber = apartmentNumber;
        }

        public override string ToString()
        {
            var result = $"{City},  Ул. {Street}, д. {Building}";
            return ApartmentNumber == null ? result : result + $", кв. {ApartmentNumber}";
        }

        protected bool Equals(Address other)
        {
            return string.Equals(City, other.City, StringComparison.InvariantCultureIgnoreCase) &&
                   string.Equals(Street, other.Street, StringComparison.InvariantCultureIgnoreCase) &&
                   string.Equals(Building, other.Building, StringComparison.InvariantCultureIgnoreCase) &&
                   ApartmentNumber == other.ApartmentNumber;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((Address) obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                var hashCode = StringComparer.InvariantCultureIgnoreCase.GetHashCode(City);
                hashCode = (hashCode * 397) ^ StringComparer.InvariantCultureIgnoreCase.GetHashCode(Street);
                hashCode = (hashCode * 397) ^ StringComparer.InvariantCultureIgnoreCase.GetHashCode(Building);
                hashCode = (hashCode * 397) ^ ApartmentNumber.GetHashCode();
                return hashCode;
            }
        }
    }
}