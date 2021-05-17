using System;
using System.Globalization;
using Martius.Infrastructure;

namespace Martius.Domain
{
    public class Lease : IDataEntity
    {
        public int Id { get; }
        public readonly RealProperty RealProperty;
        public readonly Tenant Tenant;
        public readonly decimal MonthlyPrice;
        public readonly DateTime StartDate;
        public readonly DateTime EndDate;

        public Lease(int id, RealProperty realProperty, Tenant tenant, decimal monthlyPrice, DateTime startDate,
            DateTime endDate)
        {
            Id = id;
            RealProperty = realProperty;
            Tenant = tenant;
            MonthlyPrice = monthlyPrice;
            StartDate = startDate;
            EndDate = endDate;
        }

        protected bool Equals(Lease other)
        {
            return RealProperty.Equals(other.RealProperty) && Tenant.Equals(other.Tenant) &&
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
                var hashCode = RealProperty.GetHashCode();
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
            var propId = RealProperty.Id;
            var tenantId = Tenant.Id;
            var price = MonthlyPrice.ToString(CultureInfo.InvariantCulture);
            return $"{propId}, {tenantId}, {price}, " +
                   $"'{CastUtils.FormatSqlDate(StartDate)}', '{CastUtils.FormatSqlDate(StartDate)}'";
        }

        public override string ToString()
        {
            return $"{nameof(Id)}: {Id}," +
                   $" {nameof(RealProperty)}: {RealProperty}, " +
                   $"{nameof(Tenant)}: {Tenant}, " +
                   $"{nameof(MonthlyPrice)}: {MonthlyPrice}, " +
                   $"{nameof(StartDate)}: {StartDate}, " +
                   $"{nameof(EndDate)}: {EndDate}";
        }
    }
}