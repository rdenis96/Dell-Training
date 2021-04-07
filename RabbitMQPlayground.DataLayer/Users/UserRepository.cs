using MongoDB.Driver;
using RabbitMQPlayground.DataLayer.Common;
using RabbitMQPlayground.Domain.Users;
using RabbitMQPlayground.Helpers;
using System.Threading.Tasks;

namespace RabbitMQPlayground.DataLayer.Users
{
    public class UserRepository : MongoBaseRepository<User>, IUserRepository
    {
        public UserRepository() : base(AppConfigurationBuilder.Instance.MongoSettings.Database, "Users")
        {
        }

        public async Task<User> GetByUsernameAsync(string username)
        {
            var taskResult = await _collection.FindAsync(x => x.Username == username);
            var result = await taskResult.SingleOrDefaultAsync();
            return result;
        }
    }
}