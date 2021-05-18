using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using Martius.Domain;
using Martius.Infrastructure;

namespace Martius.AppLogic
{
    public class PropertyService
    {
        public readonly List<Property> AllProperties;
        public int MaxId { get; private set; }

        public PropertyService()
        {
            AllProperties = PropertyDataManager.GetAllProperties();
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

        public Property SaveProperty(string city, string street, string buildNumber, string apartmentNumber,
            string roomData, string areaData, bool isRes, bool isFurn, bool hasPark, string priceData,
            string buildExtra = null)
        {
            var bldNum = int.Parse(buildNumber);
            var aptNum = CastUtils.ToNullableInt(apartmentNumber);
            var address = new Address(city, street, bldNum, aptNum, buildExtra);
            // todo parse complex building numbers (like 221/a)

            var roomCount = int.Parse(roomData);
            var area = double.Parse(areaData, CultureInfo.InvariantCulture);
            decimal.TryParse(priceData, NumberStyles.Any, CultureInfo.InvariantCulture, out var price);
            var property = new Property(
                MaxId + 1, address, roomCount, area, isRes, isFurn, hasPark, price);

            PropertyDataManager.AddProperty(property);
            AllProperties.Add(property);
            MaxId = property.Id;
            return property;
        }
    }
}