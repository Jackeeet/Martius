using System;
using System.Globalization;
using System.Text;
using Martius.Infrastructure;

namespace Martius.Domain
{
    public class RealProperty : Entity<int>
    {
        public Address Address { get; }
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
            var sb = new StringBuilder();
            sb.Append(Address.ToSqlString() + ", ");
            sb.Append($"{RoomCount}, {area}, {res}, {furn}, {park}, {price}");
            return sb.ToString();
        }

        internal RealProperty(int id, Address address) : base(id)
        {
            Address = address;
        }

        public RealProperty(int id, Address address, int roomCount, double area, bool isRes, bool isFurn,
            bool hasPark, decimal price) : base(id)
        {
            Address = address;
            RoomCount = roomCount;
            Area = area;
            IsResidential = isRes;
            IsFurnished = isFurn;
            HasParking = hasPark;
            MonthlyPrice = price;
        }

        public override string ToString()
        {
            return $"{base.ToString()}," +
                   $" {nameof(Address)}: {Address}," +
                   $" {nameof(RoomCount)}: {RoomCount}, " +
                   $"{nameof(Area)}: {Area}," +
                   $" {nameof(IsResidential)}: {IsResidential}," +
                   $" {nameof(IsFurnished)}: {IsFurnished}," +
                   $" {nameof(HasParking)}: {HasParking}," +
                   $" {nameof(MonthlyPrice)}: {MonthlyPrice}";
        }
    }
}