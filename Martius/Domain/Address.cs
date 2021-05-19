using System;

namespace Martius.Domain
{
    public class Address : IComparable
    {
        private readonly int _buildingNumber;
        private readonly string _buildingExtra;
        public readonly string City;
        public readonly string Street;
        public string Building => _buildingNumber.ToString() + _buildingExtra;
        public readonly int? ApartmentNumber;

        public Address(string city, string street, int buildNumber, int? apartmentNumber, string buildExtra = null)
        {
            City = city;
            Street = street;
            ApartmentNumber = apartmentNumber;
            _buildingNumber = buildNumber;
            _buildingExtra = buildExtra;
        }

        public string ToSqlString()
        {
            return $"N'{City}', N'{Street}', {_buildingNumber}, N'{_buildingExtra}', {ApartmentNumber}";
        }


        public override string ToString()
        {
            var result = $"{City}, Ул. {Street}, д. {Building}";
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

        public int CompareTo(object obj)
        {
            if (obj == null)
                return 1;

            if (obj is Address otherAddress)
            {
                // todo implement actual comparison
                return string.Compare(this.City, otherAddress.City, StringComparison.Ordinal);
            }
            else
                throw new ArgumentException("obj is not an Address");
        }
    }
}