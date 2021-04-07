using AutoMapper;
using MassTransit;
using RabbitMQPlayground.Domain.Users;
using RabbitMQPlayground.Logic.Users.Services;
using System;
using System.Threading.Tasks;

namespace RabbitMQPlayground.Logic.Services.Users.Consumers
{
    public class UserConsumer : IConsumer<IUser>, IConsumer<IUserWrapper>
    {
        private readonly IUserService _userService;
        private readonly IMapper _mapper;

        public UserConsumer(IUserService userService, IMapper mapper)
        {
            _userService = userService;
            _mapper = mapper;
        }

        public async Task Consume(ConsumeContext<IUser> context)
        {
            var user = GenerateUser(context.Message.Username, context.Message.Password);

            Console.WriteLine($"Received: user {context.Message.Username} with password {context.Message.Password}");

            await _userService.Add(user);
        }

        public async Task Consume(ConsumeContext<IUserWrapper> context)
        {
            var existingUser = await _userService.GetByIdAsync(context.Message.Id);
            if (existingUser != null)
            {
                var updatedUser = _mapper.Map<IUserWrapper, User>(context.Message);
                await _userService.Update(updatedUser);
            }
            else
            {
                var newUser = GenerateUser(context.Message.Username, context.Message.Password);
                await _userService.Add(newUser);
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