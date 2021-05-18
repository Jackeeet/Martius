using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using static Martius.Infrastructure.CastUtils;

namespace Martius.Domain
{
    // ReSharper disable once ClassNeverInstantiated.Global
    internal class PropertyDataManager : DataManager
    {
        internal static List<Property> GetAllProperties() =>
            GetAllEntities("property", BuildProperty).Cast<Property>().ToList();

        internal static Property GetPropertyById(int propId) =>
            GetEntityById(propId, "property", BuildProperty) as Property;

        internal static void AddProperty(Property prop)
        {
            var tableDesc =
                "insert into property(city, street, building, building_extra, apt_number, room_count, area, residential, furnished, has_parking, monthly_price)";
            AddEntity(prop, tableDesc);
        }

        private static Property BuildProperty(SqlDataReader reader)
        {
            var id = reader.GetInt32(0);
            var address = GetAddress(reader);
            var roomCount = reader.GetInt32(5);
            var area = reader.GetDouble(6);
            var res = reader.GetBoolean(7);
            var furn = reader.GetBoolean(8);
            var park = reader.GetBoolean(9);
            var price = reader.GetDecimal(10);
            var property = new Property(id, address, roomCount, area, res, furn, park, price);
            return property;
        }

        private static Address GetAddress(SqlDataReader reader)
        {
            var city = reader.GetString(1);
            var street = reader.GetString(2);
            var building = reader.GetInt32(3);
            var buildExtra = reader.IsDBNull(11) ? null : reader.GetString(11);
            var aptNumber = reader.IsDBNull(4) ? null : ToNullableInt(reader.GetInt32(4).ToString());
            return new Address(city, street, building, aptNumber, buildExtra);
        }
    }
}