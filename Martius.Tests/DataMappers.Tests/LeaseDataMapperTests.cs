using System;
using Martius.Domain;
using Martius.Infrastructure;
using NSubstitute;
using NUnit.Framework;

namespace Martius.Tests.DataMappers.Tests
{
    [TestFixture]
    public class LeaseDataMapperTests
    {
        private IDbConnectionFactory _connectionFactory;
        private LeaseDataMapper _mapper;

        [SetUp]
        public void SetUp()
        {
            _connectionFactory = Substitute.For<IDbConnectionFactory>();
            _mapper = new LeaseDataMapper(_connectionFactory);
        }

        [Test]
        public void Should_GetAllLeases()
        {
        }

        [Test]
        public void Should_AddLease()
        {
            var address = new Address("", "", 0, 0);
            var property = new Property(0, address, 0, 0, false, false, false, decimal.Zero);
            var person = new Person("surname", "name", "patronym", DateTime.Now);
            var tenant = new Tenant(0, person, "000", "000");
            var lease = new Lease(0, property, tenant, decimal.Zero, DateTime.Now, DateTime.Now);

            _mapper.AddLease(lease);
            _connectionFactory.Received().CreateConnection();
        }

        [Test]
        public void Should_GetFilteredLeases()
        {
        }
    }
}