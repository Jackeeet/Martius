using System;
using System.Collections.Generic;
using Martius.Domain;

namespace Martius.AppLogic
{
    public class LeaseService
    {
        public readonly List<Lease> AllLeases;
        private readonly LeaseDataManager _dataManager;
        public int MaxId { get; private set; }

        public LeaseService(string connectionString)
        {
            _dataManager = new LeaseDataManager(connectionString);
            AllLeases = _dataManager.GetAllLeases();
            var lastIndex = AllLeases.Count - 1;
            MaxId = lastIndex == -1 ? 0 : AllLeases[lastIndex].Id;
        }

        public Lease SaveLease(Property property, Tenant tenant, decimal price,
            DateTime startDate, DateTime endDate)
        {
            var lease = new Lease(MaxId + 1, property, tenant, price, startDate, endDate);

            // todo check if property is already taken
            // throw custom exception if it is
            
            _dataManager.AddLease(lease);
            AllLeases.Add(lease);
            MaxId = lease.Id;
            return lease;
        }

        public decimal GetDiscountedAmount(Property property, Tenant tenant, int minCount, decimal discount)
        {
            var leaseCount = _dataManager.GetPropertyLeaseCount(property.Id, tenant.Id);
            var multiplier = discount * new decimal(0.01);
            var actualAmount = property.MonthlyPrice - (property.MonthlyPrice * multiplier);
            return leaseCount < minCount ? decimal.Zero : actualAmount;
        }
    }
}