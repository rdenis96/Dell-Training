using RabbitMQPlayground.Domain.Common;

namespace RabbitMQPlayground.Domain.Users
{
    public interface IUserWrapper : IMongoEntity
    {
        string Username { get; set; }
        string Password { get; set; }
    }
}