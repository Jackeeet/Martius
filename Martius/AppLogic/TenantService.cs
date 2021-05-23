using System;
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
        public int MaxId { get; private set; }

        public TenantService(string connectionString)
        {
            _dataManager = new TenantDataManager(connectionString);
            Tenants = _dataManager.GetAllTenants();
            var lastIndex = Tenants.Count - 1;
            MaxId = lastIndex == -1 ? 0 : Tenants[lastIndex].Id;
        }

        public List<int> AllIds => Tenants.Select(t => t.Id).ToList();

        public List<Person> AllPeople => Tenants.Select(t => t.PersonInfo).ToList();

        public Tenant SaveTenant(Person person, string passport, string phone)
        {
            var tenant = new Tenant(MaxId + 1, person, phone, passport);

            if (!IsUnique(tenant))
                throw new EntityExistsException("Такой арендатор уже существует в базе.");

            _dataManager.AddTenant(tenant);
            Tenants.Add(tenant);
            MaxId = tenant.Id;
            return tenant;
        }

        private bool IsUnique(Tenant tenant) => Tenants.All(t => !t.PersonInfo.Equals(tenant.PersonInfo));
    }
}