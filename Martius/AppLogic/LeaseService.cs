using System;
using System.Collections.Generic;
using System.Linq;
using Martius.Domain;
using Martius.Infrastructure;

namespace Martius.AppLogic
{
    public class LeaseService
    {
        public readonly List<Lease> Leases;
        private readonly LeaseDataManager _dataManager;
        public int MaxId { get; private set; }

        public LeaseService(string connectionString)
        {
            _dataManager = new LeaseDataManager(connectionString);
            Leases = _dataManager.GetAllLeases();
            var lastIndex = Leases.Count - 1;
            MaxId = lastIndex == -1 ? 0 : Leases[lastIndex].Id;
        }

        public Lease SaveLease(Property property, Tenant tenant, decimal price,
            DateTime startDate, DateTime endDate)
        {
            var lease = new Lease(MaxId + 1, property, tenant, price, startDate, endDate);

            if (!IsUnique(lease))
                throw new EntityExistsException("Такая запись уже существует в базе.");

            // todo check if property is already taken
            // throw custom exception if it is

            _dataManager.AddLease(lease);
            Leases.Add(lease);
            MaxId = lease.Id;
            return lease;
        }

        private bool IsUnique(Lease lease) => Leases.All(l => !l.Equals(lease));

        public decimal GetDiscountedAmount(Property property, Tenant tenant, int minCount, decimal discount)
        {
            var leaseCount = _dataManager.GetPropertyLeaseCount(property.Id, tenant.Id);
            var multiplier = discount * new decimal(0.01);
            var actualAmount = property.MonthlyPrice - (property.MonthlyPrice * multiplier);
            return leaseCount < minCount ? decimal.Zero : actualAmount;
        }
    }
}