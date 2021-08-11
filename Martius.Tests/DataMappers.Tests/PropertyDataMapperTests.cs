using Martius.Domain;
using Martius.Infrastructure;
using NSubstitute;
using NUnit.Framework;

namespace Martius.Tests.DataMappers.Tests
{
    [TestFixture]
    public class PropertyDataMapperTests
    {
        private IDbConnectionFactory _connectionFactory;
        private PropertyDataMapper _mapper;

        [SetUp]
        public void SetUp()
        {
            _connectionFactory = Substitute.For<IDbConnectionFactory>();
            _mapper = new PropertyDataMapper(_connectionFactory);
        }

        [Test]
        public void Should_GetAllProperties()
        {
        }

        [Test]
        public void Should_GetPropertyById()
        {
            // Should.BeOfType<Property>();
        }

        [Test]
        public void Should_AddProperty()
        {
            var address = new Address("", "", 0, 0);
            var property = new Property(0, address, 0, 0, false, false, false, decimal.Zero);

            _mapper.AddProperty(property);
            _connectionFactory.Received().CreateConnection();
        }

        [Test]
        public void Should_UpdateProperty()
        {
            var address = new Address("", "", 0, 0);
            var property = new Property(0, address, 0, 0, false, false, false, decimal.Zero);

            _mapper.UpdateProperty(property);
            _connectionFactory.Received().CreateConnection();
        }

        [Test]
        public void Should_GetFilteredProperties()
        {
        }
    }
}