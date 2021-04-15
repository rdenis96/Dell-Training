using RabbitMQPlayground.Domain.Users;
using RabbitMQPlayground.Logic.Common;
using System.Threading.Tasks;

namespace RabbitMQPlayground.Logic.Users
{
    public interface IUserService : IBaseService<User>
    {
        Task<User> GetByUsernameAsync(string username);
    }
}