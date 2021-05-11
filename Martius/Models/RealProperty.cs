using Martius.Infrastructure;

namespace Martius.Models
{
    public class RealProperty : Entity<int>
    {
        public readonly Address Address;
        public int RoomCount { get; set; }
        public double Area { get; set; }
        public bool IsResidential { get; set; }
        public bool IsFurnished { get; set; }
        public bool HasParking { get; set; }
        public int? YearBuilt { get; set; }
        
        public RealProperty(int id, Address address) : base(id)
        {
            Address = address;
        }
        
        
    }
}