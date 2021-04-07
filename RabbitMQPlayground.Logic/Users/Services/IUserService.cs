using MongoDB.Bson;
using RabbitMQPlayground.Domain.Users;
using System.Threading.Tasks;

namespace RabbitMQPlayground.Logic.Users.Services
{
    public interface IUserService
    {
        Task<User> GetByUsernameAsync(string username);

        Task<User> GetByIdAsync(ObjectId id);

        Task Add(User user);

        Task Update(User user);
    }
}