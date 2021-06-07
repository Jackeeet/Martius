using System.Collections.Generic;
using System.Linq;
using Martius.Domain;
using Martius.Infrastructure;

namespace Martius.AppLogic
{
    public class TenantService
    {
        public readonly List<Tenant> Tenants;
        private readonly TenantDataManager _dataManager;
        private int _maxId;

        public TenantService(string connectionString)
        {
            _dataManager = new TenantDataManager(connectionString);
            Tenants = _dataManager.GetAllTenants();
            var lastIndex = Tenants.Count - 1;
            _maxId = lastIndex == -1 ? 0 : Tenants[lastIndex].Id;
        }

        public List<Tenant> GetFilteredTenants(string filter, string join = null)
            => _dataManager.GetFilteredTenants(filter, join);

        public void UpdateTenant(Tenant tenant) => _dataManager.UpdateTenant(tenant);

        public Tenant SaveTenant(Person person, string passport, string phone)
        {
            var tenant = new Tenant(_maxId + 1, person, phone, passport);

            if (!IsUnique(tenant))
                throw new EntityExistsException("Такой арендатор уже существует в базе.");

            _dataManager.AddTenant(tenant);
            Tenants.Add(tenant);
            _maxId = tenant.Id;
            return tenant;
        }

        private bool IsUnique(Tenant tenant) => Tenants.All(t => !t.PersonInfo.Equals(tenant.PersonInfo));
    }
}