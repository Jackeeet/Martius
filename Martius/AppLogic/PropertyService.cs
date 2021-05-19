using System.Collections.Generic;
using System.Linq;
using Martius.Domain;

namespace Martius.AppLogic
{
    public class PropertyService
    {
        public readonly List<Property> AllProperties;
        private readonly PropertyDataManager _dataManager;
        public int MaxId { get; private set; }

        public PropertyService(string connectionString)
        {
            _dataManager = new PropertyDataManager(connectionString);
            AllProperties = _dataManager.GetAllProperties();
            var lastIndex = AllProperties.Count - 1;
            MaxId = lastIndex == -1 ? 0 : AllProperties[lastIndex].Id;
        }

        public List<string> AllCities
        {
            get
            {
                return AllProperties
                    .Select(p => p.Address.City)
                    .Distinct()
                    .OrderBy(c => c)
                    .ToList();
            }
        }

        public List<int> AllIds => AllProperties.Select(p => p.Id).ToList();

        public Property SaveProperty(
            Address address, int roomCount, double area, bool isRes, bool isFurn, bool hasPark, decimal price)
        {
            var property = new Property(MaxId + 1, address, roomCount, area, isRes, isFurn, hasPark, price);

            _dataManager.AddProperty(property);
            AllProperties.Add(property);
            MaxId = property.Id;
            return property;
        }
    }
}