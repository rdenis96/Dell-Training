using RabbitMQPlayground.Domain.Users;

namespace RabbitMQPlayground.API.Models
{
    public class UserDto : IUser
    {
        public string Username { get; set; }
        public string Password { get; set; }
    }
}