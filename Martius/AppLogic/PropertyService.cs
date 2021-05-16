using System.Collections.Generic;
using System.Globalization;
using Martius.Domain;
using Martius.Infrastructure;

namespace Martius.AppLogic
{
    public class PropertyService
    {
        private List<RealProperty> _allProperties;
        public int MaxId { get; private set; }

        public List<RealProperty> GetAllProperties()
        {
            _allProperties = DataManager.GetAllRealProperties();
            MaxId = _allProperties[_allProperties.Count - 1].Id;
            return _allProperties;
        }

        public void SaveProperty(string city, string street, string buildNumber, string apartmentNumber,
            string roomData, string areaData, bool isRes, bool isFurn, bool hasPark, string priceData,
            string buildExtra = null)
        {
            var bldNum = int.Parse(buildNumber);
            var aptNum = ParseUtilities.ToNullableInt(apartmentNumber);
            var address = new Address(city, street, bldNum, aptNum, buildExtra);
            // todo parse complex building numbers (like 221/a)

            var roomCount = int.Parse(roomData);
            var area = double.Parse(areaData, CultureInfo.InvariantCulture);
            decimal.TryParse(priceData, NumberStyles.Any, CultureInfo.InvariantCulture, out var price);
            var property = new RealProperty(
                MaxId + 1, address, roomCount, area, isRes, isFurn, hasPark, price);

            DataManager.AddProperty(property);
        }
    }
}