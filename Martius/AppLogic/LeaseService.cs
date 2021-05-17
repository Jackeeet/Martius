using System;
using System.Collections.Generic;
using Martius.Domain;

namespace Martius.AppLogic
{
    public class LeaseService
    {
        public readonly List<Lease> AllLeases;
        public int MaxId { get; private set; }

        public LeaseService()
        {
            AllLeases = DataManager.GetAllLeases();
            var lastIndex = AllLeases.Count - 1;
            MaxId = lastIndex == -1 ? 0 : AllLeases[lastIndex].Id;
        }

        public Lease SaveLease(RealProperty property, Tenant tenant, decimal price,
            DateTime startDate, DateTime endDate)
        {
            var lease = new Lease(MaxId + 1, property, tenant, price, startDate, endDate);

            DataManager.AddLease(lease);
            MaxId = lease.Id;
            return lease;
        }
    }
}