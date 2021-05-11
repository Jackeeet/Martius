using Martius.Infrastructure;

namespace Martius.Models
{
    public class Tenant : Entity<int>
    {
        public Person PersonInfo { get; set; }
        public string PhoneNumber { get; set; }
        public string PassportNumber { get; set; }
        
        public Tenant(int id, Person personInfo, string phoneNumber, string passportNumber) : base(id)
        {
            PersonInfo = personInfo;
            PhoneNumber = phoneNumber;
            PassportNumber = passportNumber;
        }
    }
}