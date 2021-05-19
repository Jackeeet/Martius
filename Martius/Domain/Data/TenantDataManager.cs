using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;

namespace Martius.Domain
{
    internal class TenantDataManager : DataManager
    {
        internal List<Tenant> GetAllTenants() =>
            GetAllEntities("tenant", BuildTenant).Cast<Tenant>().ToList();

        internal Tenant GetTenantById(int tenantId) =>
            GetEntityById(tenantId, "tenant", BuildTenant) as Tenant;

        internal void AddTenant(Tenant tenant)
        {
            var tableDesc = "insert into tenant(surname, name, patronym, dob, phone, passport)";
            AddEntity(tenant, tableDesc);
        }

        private static Tenant BuildTenant(SqlDataReader reader)
        {
            var id = reader.GetInt32(0);
            var person = GetPerson(reader);
            var passport = reader.IsDBNull(5) ? null : reader.GetString(5);
            var phone = reader.IsDBNull(6) ? null : reader.GetString(6);

            var tenant = new Tenant(id, person, phone, passport);
            return tenant;
        }

        private static Person GetPerson(SqlDataReader reader)
        {
            var surname = reader.GetString(1);
            var name = reader.GetString(2);
            var patronym = reader.IsDBNull(3) ? null : reader.GetString(3);
            var dob = reader.GetDateTime(4);
            return new Person(surname, name, patronym, dob);
        }

        protected internal TenantDataManager(string connectionString) : base(connectionString)
        {
        }
    }
}