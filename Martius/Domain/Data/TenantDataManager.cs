using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Runtime.CompilerServices;

[assembly: InternalsVisibleTo("LibTestProject")]

namespace Martius.Domain
{
    internal class TenantDataManager : DataManager
    {
        private string _tableName = "tenant";
        private string _tableColumns = "surname, name, patronym, dob, phone, passport";

        internal List<Tenant> GetAllTenants() => GetEntities(_tableName, BuildEntity).Cast<Tenant>().ToList();

        internal Tenant GetTenantById(int tenantId) =>
            GetEntities(_tableName, BuildEntity, $"where id = {tenantId}").FirstOrDefault() as Tenant;

        internal void AddTenant(Tenant tenant) => AddEntity(tenant, _tableName, _tableColumns);

        internal List<Tenant> GetFilteredTenants(string filter, string join = null) =>
            GetEntities(_tableName, BuildEntity, filter, join).Cast<Tenant>().ToList();

        internal void UpdateTenant(Tenant tenant) => UpdateEntity(tenant, _tableName, _tableColumns);


        protected override IDataEntity BuildEntity(SqlDataReader reader)
        {
            var id = reader.GetInt32(0);
            var person = GetPerson(reader);
            var passport = reader.IsDBNull(5) ? null : reader.GetString(5);
            var phone = reader.IsDBNull(6) ? null : reader.GetString(6);

            return new Tenant(id, person, phone, passport);
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