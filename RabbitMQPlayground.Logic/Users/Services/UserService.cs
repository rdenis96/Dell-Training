using MongoDB.Bson;
using RabbitMQPlayground.DataLayer.Users;
using RabbitMQPlayground.Domain.Users;
using System.Threading.Tasks;

namespace RabbitMQPlayground.Logic.Users.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;

        public UserService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task Add(User user)
        {
            try
            {
                await _userRepository.CreateAsync(user);
            }
            catch
            {
                throw;
            }
        }

        public async Task Update(User user)
        {
            try
            {
                await _userRepository.UpdateAsync(user);
            }
            catch
            {
                throw;
            }
        }

        public async Task<User> GetByIdAsync(ObjectId id)
        {
            User result = await _userRepository.GetByIdAsync(id);

            return result;
        }

        public async Task<User> GetByUsernameAsync(string username)
        {
            User result = await _userRepository.GetByUsernameAsync(username);

            return result;
        }
    }
}