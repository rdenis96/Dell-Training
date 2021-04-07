using RabbitMQPlayground.DataLayer.Common;
using RabbitMQPlayground.Domain.Users;
using System.Threading.Tasks;

namespace RabbitMQPlayground.DataLayer.Users
{
    public interface IUserRepository : IMongoBaseRepository<User>
    {
        Task<User> GetByUsernameAsync(string username);
    }
}