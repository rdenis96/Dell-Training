using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using RabbitMQPlayground.DataLayer.Users;
using RabbitMQPlayground.Domain.Users;
using System.Threading.Tasks;

namespace RabbitMQPlayground.MongoAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserRepository _userRepository;
        private readonly ILogger<UserController> _logger;

        public UserController(IUserRepository userRepository, ILogger<UserController> logger)
        {
            _userRepository = userRepository;
            _logger = logger;
        }

        [HttpPost("[action]")]
        [ProducesResponseType(typeof(User), 200)]
        public async Task<IActionResult> Create([FromBody] User user)
        {
            var result = await _userRepository.CreateAsync(user);
            return Ok(result);
        }

        [HttpPut("[action]")]
        [ProducesResponseType(typeof(User), 200)]
        public async Task<IActionResult> Update([FromBody] User user)
        {
            var result = await _userRepository.UpdateAsync(user);
            return Ok(result);
        }

        [HttpGet("[action]")]
        [ProducesResponseType(typeof(User), 200)]
        public async Task<IActionResult> GetByUsername([FromQuery] string username)
        {
            User result = await _userRepository.GetByUsernameAsync(username);
            return Ok(result);
        }
    }
}