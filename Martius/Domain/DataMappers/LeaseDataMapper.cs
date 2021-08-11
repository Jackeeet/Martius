using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Runtime.CompilerServices;
using Martius.Infrastructure;

[assembly: InternalsVisibleTo("Martius.Tests")]

namespace Martius.Domain
{
    internal class LeaseDataMapper : DataMapper
    {
        private string _tableName = "lease";
        private string _tableColumns = "property_id, tenant_id, monthly_price, start_date, end_date";

        internal List<Lease> GetAllLeases() => GetEntities(_tableName).Cast<Lease>().ToList();

        internal void AddLease(Lease lease) => AddEntity(lease, _tableName, _tableColumns);

        internal List<Lease> GetFilteredLeases(string filter, string join = null) =>
            GetEntities(_tableName, filter, join).Cast<Lease>().ToList();

        protected override IDataEntity BuildEntity(SqlDataReader reader)
        {
            var id = reader.GetInt32(0);
            var propId = reader.GetInt32(1);
            var tenantId = reader.GetInt32(2);
            var monthlyPrice = reader.GetDecimal(3);
            var startDate = reader.GetDateTime(4);
            var endDate = reader.GetDateTime(5);

            var prop = new PropertyDataMapper(ConnectionFactory).GetPropertyById(propId);
            var tenant = new TenantDataMapper(ConnectionFactory).GetTenantById(tenantId);

            return new Lease(id, prop, tenant, monthlyPrice, startDate, endDate);
        }

        protected internal LeaseDataMapper(IDbConnectionFactory connectionFactory) : base(connectionFactory)
        {
        }
    }
}