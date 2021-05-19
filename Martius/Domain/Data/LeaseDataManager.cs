using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Runtime.CompilerServices;

[assembly: InternalsVisibleTo("LibTestProject")]

namespace Martius.Domain
{
    internal class LeaseDataManager : DataManager
    {
        private string _tableName = "lease";
        private string _tableColumns = "property_id, tenant_id, monthly_price, start_date, end_date";

        internal List<Lease> GetAllLeases() => GetAllEntities(_tableName, BuildEntity).Cast<Lease>().ToList();

        internal Lease GetLeaseById(int leaseId) => GetEntityById(leaseId, _tableName, BuildEntity) as Lease;

        internal void AddLease(Lease lease) => AddEntity(lease, _tableName, _tableColumns);

        protected override IDataEntity BuildEntity(SqlDataReader reader)
        {
            var id = reader.GetInt32(0);
            var propId = reader.GetInt32(1);
            var tenantId = reader.GetInt32(2);
            var monthlyPrice = reader.GetDecimal(3);
            var startDate = reader.GetDateTime(4);
            var endDate = reader.GetDateTime(5);

            var prop = new PropertyDataManager(ConnectionString).GetPropertyById(propId);
            var tenant = new TenantDataManager(ConnectionString).GetTenantById(tenantId);

            return new Lease(id, prop, tenant, monthlyPrice, startDate, endDate);
        }

        internal int GetPropertyLeaseCount(int propId, int tId)
        {
            // todo prop check should probably be more elaborate, check reqs
            var count = 0;
            var connection = new SqlConnection(ConnectionString);
            var com = $"select count(id) from lease where property_id = {propId} and tenant_id = {tId}";
            using (connection)
            {
                var command = new SqlCommand(com, connection);
                try
                {
                    connection.Open();
                    count = (int) command.ExecuteScalar();
                }
                catch (SqlException e)
                {
                    Console.WriteLine(e.Message);
                }
            }

            return count;
        }

        protected internal LeaseDataManager(string connectionString) : base(connectionString)
        {
        }
    }
}