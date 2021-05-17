using System;
using System.Text;

namespace Martius.Domain
{
    public class Tenant : IDataEntity
    {
        public int Id { get; }

        public Person PersonInfo { get; set; }
        public string PhoneNumber { get; set; }
        public string PassportNumber { get; set; }

        public Tenant(int id, Person personInfo, string phoneNumber, string passportNumber)
        {
            Id = id;
            PersonInfo = personInfo;
            PhoneNumber = phoneNumber;
            PassportNumber = passportNumber;
        }

        public string ToSqlString()
        {
            return $"{PersonInfo.ToSqlString()}, '{PhoneNumber}', '{PassportNumber}'";
        }

        protected bool Equals(Tenant other) => Id == other.Id;

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((Tenant) obj);
        }

        public override int GetHashCode() => Id;

        public override string ToString()
        {
            return $"{nameof(Id)}: {Id}," +
                   $" {nameof(PersonInfo)}: {PersonInfo}, " +
                   $"{nameof(PhoneNumber)}: {PhoneNumber}, " +
                   $"{nameof(PassportNumber)}: {PassportNumber}";
        }
    }
}