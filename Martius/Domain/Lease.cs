using System;
using System.Globalization;
using Martius.Infrastructure.Extensions;

namespace Martius.Domain
{
    public class Lease : IDataEntity
    {
        public int Id { get; }
        public Property Property { get; }
        public Tenant Tenant { get; }
        public decimal MonthlyPrice { get; }
        public DateTime StartDate { get; }
        public DateTime EndDate { get; }

        public Address Address => Property.Address;
        public string TenantName => Tenant.FullName;
        public decimal FullPrice => MonthlyPrice * _months;
        private readonly int _months;

        public Lease(int id, Property property, Tenant tenant, decimal monthlyPrice, DateTime startDate,
            DateTime endDate)
        {
            Id = id;
            Property = property;
            Tenant = tenant;
            MonthlyPrice = monthlyPrice;
            StartDate = startDate;
            EndDate = endDate;
            _months = StartDate.CalculateMonthsUntil(EndDate);
        }

        public bool ContentEquals(Lease other)
        {
            return Property.Equals(other.Property) && Tenant.Equals(other.Tenant) &&
                   MonthlyPrice == other.MonthlyPrice && StartDate.Equals(other.StartDate) &&
                   EndDate.Equals(other.EndDate);
        }

        protected bool Equals(Lease other)
        {
            return Property.Equals(other.Property) && Tenant.Equals(other.Tenant) &&
                   MonthlyPrice == other.MonthlyPrice && StartDate.Equals(other.StartDate) &&
                   EndDate.Equals(other.EndDate) && Id == other.Id;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((Lease) obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                var hashCode = Property.GetHashCode();
                hashCode = (hashCode * 397) ^ Tenant.GetHashCode();
                hashCode = (hashCode * 397) ^ MonthlyPrice.GetHashCode();
                hashCode = (hashCode * 397) ^ StartDate.GetHashCode();
                hashCode = (hashCode * 397) ^ EndDate.GetHashCode();
                hashCode = (hashCode * 397) ^ Id;
                return hashCode;
            }
        }

        public string ToSqlString()
        {
            var propId = Property.Id;
            var tenantId = Tenant.Id;
            var price = MonthlyPrice.ToString(CultureInfo.InvariantCulture);
            return $"{propId}, {tenantId}, {price}, " +
                   $"{StartDate.GetSqlRepresentation()}, {EndDate.GetSqlRepresentation()}";
        }

        public override string ToString()
        {
            return $"{nameof(Id)}: {Id}," +
                   $" {nameof(Property)}: {Property}, " +
                   $"{nameof(Tenant)}: {Tenant}, " +
                   $"{nameof(MonthlyPrice)}: {MonthlyPrice}, " +
                   $"{nameof(StartDate)}: {StartDate}, " +
                   $"{nameof(EndDate)}: {EndDate}";
        }
    }
}