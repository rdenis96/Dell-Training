using AutoMapper;
using MassTransit;
using Microsoft.Extensions.Logging;
using RabbitMQPlayground.DataLayer.Common;
using RabbitMQPlayground.Domain.Users;
using RabbitMQPlayground.Logic.Users;
using System;
using System.Threading.Tasks;

namespace RabbitMQPlayground.ConsumerService.Consumers
{
    public class UserConsumer : IConsumer<IUser>, IConsumer<IUserWrapper>
    {
        private readonly IUserService _userService;
        private readonly IMapper _mapper;
        private readonly ILogger<UserConsumer> _logger;

        public UserConsumer(ICompositionRoot compositionRoot, IMapper mapper, ILogger<UserConsumer> logger)
        {
            _userService = compositionRoot.GetImplementation<IUserService>();
            _mapper = mapper;
            _logger = logger;
        }

        public async Task Consume(ConsumeContext<IUser> context)
        {
            _logger.LogInformation($"Consuming context of {nameof(IUser)} type");

            var user = GenerateUser(context.Message.Username, context.Message.Password);

            _logger.LogTrace($"Received: user {context.Message.Username} with password {context.Message.Password}");
            try
            {
                await _userService.Add(user);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
            }
        }

        public async Task Consume(ConsumeContext<IUserWrapper> context)
        {
            _logger.LogInformation($"Consuming context of {nameof(IUserWrapper)} type");

            var updatedUser = _mapper.Map<IUserWrapper, User>(context.Message);

            _logger.LogInformation($"Updating user!");

            try
            {
                await _userService.Update(updatedUser);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
            }
        }

        private static User GenerateUser(string username, string password)
        {
            return new User
            {
                Username = username,
                Password = password
            };
        }
    }
}