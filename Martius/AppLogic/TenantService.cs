using System.Collections.Generic;
using System.Linq;
using Martius.Domain;
using Martius.Infrastructure;

namespace Martius.AppLogic
{
    public class TenantService
    {
        public readonly List<Tenant> Tenants;
        private readonly TenantDataMapper _dataMapper;
        private int _maxId;

        public TenantService(IDbConnectionFactory connectionFactory)
        {
            _dataMapper = new TenantDataMapper(connectionFactory);
            Tenants = _dataMapper.GetAllTenants();
            var lastIndex = Tenants.Count - 1;
            _maxId = lastIndex == -1 ? 0 : Tenants[lastIndex].Id;
        }

        public List<Tenant> GetFilteredTenants(string filter, string join = null)
            => _dataMapper.GetFilteredTenants(filter, join);

        public void UpdateTenant(Tenant tenant) => _dataMapper.UpdateTenant(tenant);

        public Tenant SaveTenant(Person person, string passport, string phone)
        {
            var tenant = new Tenant(_maxId + 1, person, phone, passport);

            if (!IsUnique(tenant))
                throw new EntityExistsException("Такой арендатор уже существует в базе.");

            _dataMapper.AddTenant(tenant);
            Tenants.Add(tenant);
            _maxId = tenant.Id;
            return tenant;
        }

        private bool IsUnique(Tenant tenant) => Tenants.All(t => !t.PersonInfo.Equals(tenant.PersonInfo));
    }
}