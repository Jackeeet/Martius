using System;
using System.Collections.Generic;
using System.Linq;
using Martius.Domain;

namespace Martius.AppLogic
{
    public class TenantService
    {
        public readonly List<Tenant> AllTenants;
        public int MaxId { get; private set; }

        public TenantService()
        {
            AllTenants = DataManager.GetAllTenants();
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

            DataManager.AddTenant(tenant);
            MaxId = tenant.Id;
            return tenant;
        }
    }
}