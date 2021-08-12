using Martius.Domain;
using Martius.Infrastructure;
using NSubstitute;
using NUnit.Framework;

namespace Martius.Tests.DataMappers.Tests
{
    [TestFixture]
    public class TenantDataMapperTests
    {
        private IDbConnectionFactory _connectionFactory;
        private TenantDataMapper _mapper;

        [SetUp]
        public void SetUp()
        {
            _connectionFactory = Substitute.For<IDbConnectionFactory>();
            _mapper = new TenantDataMapper(_connectionFactory);
        }
        
        
    }
}