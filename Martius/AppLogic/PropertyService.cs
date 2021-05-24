using System.Collections.Generic;
using System.Linq;
using Martius.Domain;
using Martius.Infrastructure;

namespace Martius.AppLogic
{
    public class PropertyService
    {
        public readonly List<Property> Properties;
        private readonly PropertyDataManager _dataManager;
        public int MaxId { get; private set; }

        public PropertyService(string connectionString)
        {
            _dataManager = new PropertyDataManager(connectionString);
            Properties = _dataManager.GetAllProperties();
            var lastIndex = Properties.Count - 1;
            MaxId = lastIndex == -1 ? 0 : Properties[lastIndex].Id;
        }

        public List<string> AllCities
        {
            get
            {
                return Properties
                    .Select(p => p.Address.City)
                    .Distinct()
                    .OrderBy(c => c)
                    .ToList();
            }
        }

        public List<int> AllIds => Properties.Select(p => p.Id).ToList();

        public List<Property> GetFilteredProperties(string filter, string join = null)
            => _dataManager.GetFilteredProperties(filter, join);

        public Property SaveProperty(
            Address address, int roomCount, double area, bool isRes, bool isFurn, bool hasPark, decimal price)
        {
            var property = new Property(MaxId + 1, address, roomCount, area, isRes, isFurn, hasPark, price);

            if (!IsUnique(property))
                throw new EntityExistsException("Помещение с таким адресом уже существует в базе.");

            _dataManager.AddProperty(property);
            Properties.Add(property);
            MaxId = property.Id;
            return property;
        }

        private bool IsUnique(Property prop) => Properties.All(p => !p.Address.Equals(prop.Address));
    }
}