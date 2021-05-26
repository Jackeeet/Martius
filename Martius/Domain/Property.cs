using System;
using System.Globalization;

namespace Martius.Domain
{
    public class Property : IDataEntity
    {
        public int Id { get; }
        public Address Address { get; set; }
        public int RoomCount { get; set; }
        public double Area { get; set; }
        public bool IsResidential { get; set; }
        public bool IsFurnished { get; set; }
        public bool HasParking { get; set; }
        public decimal MonthlyPrice { get; set; }

        public string ToSqlString()
        {
            var area = Area.ToString(CultureInfo.InvariantCulture);
            var res = Convert.ToByte(IsResidential);
            var furn = Convert.ToByte(IsFurnished);
            var park = Convert.ToByte(HasParking);
            var price = MonthlyPrice.ToString(CultureInfo.InvariantCulture);
            return $"{Address.ToSqlString()}, {RoomCount}, {area}, {res}, {furn}, {park}, {price}";
        }

        public Property(int id, Address address, int roomCount, double area, bool isRes, bool isFurn,
            bool hasPark, decimal price)
        {
            Id = id;
            Address = address;
            RoomCount = roomCount;
            Area = area;
            IsResidential = isRes;
            IsFurnished = isFurn;
            HasParking = hasPark;
            MonthlyPrice = price;
        }

        protected bool Equals(Property other)
        {
            return Id == other.Id && Equals(Address, other.Address);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((Property) obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return (Id * 397) ^ (Address != null ? Address.GetHashCode() : 0);
            }
        }

        public override string ToString() => Address.ToString();

        public string ToDebugString()
        {
            return
                $"{nameof(Id)}: {Id}," +
                $" {nameof(Address)}: {Address}, " +
                $"{nameof(RoomCount)}: {RoomCount}, " +
                $"{nameof(Area)}: {Area}, " +
                $"{nameof(IsResidential)}: " +
                $"{IsResidential}, " +
                $"{nameof(IsFurnished)}: " +
                $"{IsFurnished}," +
                $" {nameof(HasParking)}:" +
                $" {HasParking}, " +
                $"{nameof(MonthlyPrice)}: {MonthlyPrice}";
        }
    }
}