using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;

namespace Martius.Domain
{
    internal class LeaseDataManager : DataManager
    {
        protected internal LeaseDataManager(string connectionString) : base(connectionString)
        {
        }

        internal List<Lease> GetAllLeases() =>
            GetAllEntities("lease", BuildLease).Cast<Lease>().ToList();

        internal Lease GetLeaseById(int leaseId) =>
            GetEntityById(leaseId, "lease", BuildLease) as Lease;

        internal void AddLease(Lease lease)
        {
            var tableDesc = "insert into lease(property_id, tenant_id, monthly_price, start_date, end_date)";
            AddEntity(lease, tableDesc);
        }

        private Lease BuildLease(SqlDataReader reader)
        {
            var id = reader.GetInt32(0);
            var propId = reader.GetInt32(1);
            var tenantId = reader.GetInt32(2);
            var monthlyPrice = reader.GetDecimal(3);
            var startDate = reader.GetDateTime(4);
            var endDate = reader.GetDateTime(5);

            var prop = new PropertyDataManager(ConnectionString).GetPropertyById(propId);
            var tenant = new TenantDataManager(ConnectionString).GetTenantById(tenantId);

            var lease = new Lease(id, prop, tenant, monthlyPrice, startDate, endDate);
            return lease;
        }

        internal int GetPropertyLeaseCount(int propId, int tId)
        {
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
    }
}