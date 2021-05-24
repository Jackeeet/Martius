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

            if (!IsAvailable(property, lease))
                throw new PropertyRentedException("Выбранное помещение занято в указанный период времени.");

            if (!IsUnique(lease))
                throw new EntityExistsException("Такая запись уже существует в базе.");

            _dataManager.AddLease(lease);
            Leases.Add(lease);
            MaxId = lease.Id;
            return lease;
        }

        public List<Lease> GetFilteredLeases(string filter, string join = null) 
            => _dataManager.GetFilteredLeases(filter, @join);

        private bool IsUnique(Lease lease) => Leases.All(l => !l.ContentEquals(lease));

        private bool IsAvailable(Property prop, Lease newLease)
        {
            var leases = _dataManager.GetLeasesWithProperty(prop);
            return leases.All(l
                => !DatePeriodsOverlap(l.StartDate, l.EndDate, newLease.StartDate, newLease.EndDate));
        }

        private bool DatePeriodsOverlap(DateTime sd1, DateTime ed1, DateTime sd2, DateTime ed2)
            => sd2 > ed1 || sd1 > ed2;

        public decimal GetDiscountedAmount(Property property, Tenant tenant, int minCount, decimal discount)
        {
            var leaseCount = _dataManager.GetPropertyLeaseCount(property.Id, tenant.Id);
            var multiplier = discount * new decimal(0.01);
            var actualAmount = property.MonthlyPrice - (property.MonthlyPrice * multiplier);
            return leaseCount < minCount ? decimal.Zero : actualAmount;
        }
    }
}