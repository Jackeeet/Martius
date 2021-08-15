using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using Martius.Infrastructure;
using Martius.Infrastructure.Extensions;

namespace Martius.Domain
{
    internal class PropertyDataMapper : DataMapper
    {
        private string _tableName = "property";

        private string _tableColumns =
            "city, street, building, building_extra, apt_number, " +
            "room_count, area, residential, furnished, has_parking, monthly_price";

        internal List<Property> GetAllProperties() => GetEntities(_tableName).Cast<Property>().ToList();

        internal Property GetPropertyById(int propId) =>
            GetEntities(_tableName, $"where id = {propId}").FirstOrDefault() as Property;

        internal void AddProperty(Property prop) => AddEntity(prop, _tableName, _tableColumns);

        internal List<Property> GetFilteredProperties(string filter, string join = null) =>
            GetEntities(_tableName, filter, join).Cast<Property>().ToList();

        internal void UpdateProperty(Property property) => UpdateEntity(property, _tableName, _tableColumns);

        protected override IDataEntity BuildEntity(SqlDataReader reader)
        {
            var id = reader.GetInt32(0);
            var address = GetAddress(reader);
            var roomCount = reader.GetInt32(5);
            var area = reader.GetDouble(6);
            var res = reader.GetBoolean(7);
            var furn = reader.GetBoolean(8);
            var park = reader.GetBoolean(9);
            var price = reader.GetDecimal(10);
            return new Property(id, address, roomCount, area, res, furn, park, price);
        }

        private static Address GetAddress(SqlDataReader reader)
        {
            var city = reader.GetString(1);
            var street = reader.GetString(2);
            var building = reader.GetInt32(3);
            var buildExtra = reader.IsDBNull(11) ? null : reader.GetString(11);
            // var aptNumber = reader.IsDBNull(4) ? null : ToNullableInt(reader.GetInt32(4).ToString());
            var aptNumber = reader.IsDBNull(4) ? null : reader.GetInt32(4).ToString().ToNullableInt();
            return new Address(city, street, building, aptNumber, buildExtra);
        }

        protected internal PropertyDataMapper(IDbConnectionFactory connectionFactory) : base(connectionFactory)
        {
        }
    }
}