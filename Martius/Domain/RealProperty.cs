using Martius.Infrastructure;

namespace Martius.Domain
{
    public class RealProperty : Entity<int>
    {
        public readonly Address Address;
        public int RoomCount { get; set; }
        public double Area { get; set; }
        public bool IsResidential { get; set; }
        public bool IsFurnished { get; set; }

        public bool HasParking { get; set; }
        //public int? YearBuilt { get; set; }

        public RealProperty(int id, Address address) : base(id)
        {
            Address = address;
        }

        public RealProperty(int id, Address address, int roomCount, double area, bool isRes, bool isFurn,
            bool hasPark) : base(id)
        {
            Address = address;
            RoomCount = roomCount;
            Area = area;
            IsResidential = isRes;
            IsFurnished = isFurn;
            HasParking = hasPark;
        }

        public override string ToString()
        {
            return
                $"{nameof(Address)}: {Address}, {nameof(RoomCount)}: {RoomCount}, {nameof(Area)}: {Area}, {nameof(IsResidential)}: {IsResidential}, {nameof(IsFurnished)}: {IsFurnished}, {nameof(HasParking)}: {HasParking}";
        }
    }
}