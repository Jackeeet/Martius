using System;
using System.Collections.Generic;
using System.Linq;
using Martius.Domain;

namespace Martius.AppLogic
{
    public class TenantService
    {
        public readonly List<Tenant> AllTenants;
        private readonly TenantDataManager _dataManager;
        public int MaxId { get; private set; }

        public TenantService(string connectionString)
        {
            _dataManager = new TenantDataManager(connectionString);
            AllTenants = _dataManager.GetAllTenants();
            var lastIndex = AllTenants.Count - 1;
            MaxId = lastIndex == -1 ? 0 : AllTenants[lastIndex].Id;
        }

        public List<int> AllIds => AllTenants.Select(t => t.Id).ToList();

        public List<Person> AllPeople => AllTenants.Select(t => t.PersonInfo).ToList();

        public Tenant SaveTenant(string surname, string name, string patronym,
            DateTime dob, string passport, string phone)
        {
            var person = new Person(surname, name, patronym, dob);
            var tenant = new Tenant(MaxId + 1, person, phone, passport);

            _dataManager.AddTenant(tenant);
            AllTenants.Add(tenant);
            MaxId = tenant.Id;
            return tenant;
        }
    }
}