using MassTransit;
using NSubstitute;
using RabbitMQPlayground.DataLayer.Users;

namespace RabbitMQPlayground.Tests.Users
{
    public class UsersTests
    {
        private readonly IUserRepository _userRepository;
        private readonly IBusControl _mockBusControl;

        public UsersTests()
        {
            _userRepository = Substitute.For<IUserRepository>();
        }
    }
}