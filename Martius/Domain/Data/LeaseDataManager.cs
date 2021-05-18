using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;

namespace Martius.Domain
{
    // ReSharper disable once ClassNeverInstantiated.Global
    internal class LeaseDataManager : DataManager
    {
        internal static List<Lease> GetAllLeases() =>
            GetAllEntities("lease", BuildLease).Cast<Lease>().ToList();

        internal static Lease GetLeaseById(int leaseId) =>
            GetEntityById(leaseId, "lease", BuildLease) as Lease;

        internal static void AddLease(Lease lease)
        {
            var tableDesc = "insert into lease(property_id, tenant_id, monthly_price, start_date, end_date)";
            AddEntity(lease, tableDesc);
        }

        private static Lease BuildLease(SqlDataReader reader)
        {
            var id = reader.GetInt32(0);
            var propId = reader.GetInt32(1);
            var tenantId = reader.GetInt32(2);
            var monthlyPrice = reader.GetDecimal(3);
            var startDate = reader.GetDateTime(4);
            var endDate = reader.GetDateTime(5);
            
            var prop = PropertyDataManager.GetPropertyById(propId);
            var tenant = TenantDataManager.GetTenantById(tenantId);

            var lease = new Lease(id, prop, tenant, monthlyPrice, startDate, endDate);
            return lease;
        }
    }
}