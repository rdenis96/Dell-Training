using MassTransit;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using RabbitMQPlayground.API.Models;
using RabbitMQPlayground.DataLayer.Common;
using RabbitMQPlayground.Domain.Users;
using RabbitMQPlayground.Logic.Users;
using System.Threading.Tasks;

namespace RabbitMQPlayground.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IBus _bus;
        private readonly IUserService _userService;
        private readonly ILogger<UserController> _logger;

        public UserController(IBus bus, ICompositionRoot compositionRoot, ILogger<UserController> logger)
        {
            _bus = bus;
            _userService = compositionRoot.GetImplementation<IUserService>();
            _logger = logger;
        }

        [HttpPost("[action]")]
        public async Task<OkResult> Add([FromBody] UserDto user)
        {
            await _bus.Publish<IUser>(user);
            return Ok();
        }

        [HttpPut("[action]")]
        public async Task<OkResult> Update([FromBody] UserWrapper user)
        {
            await _bus.Publish<IUserWrapper>(user);
            return Ok();
        }

        [HttpGet("[action]")]
        [ProducesResponseType(typeof(User), 200)]
        public async Task<ActionResult> GetByUsername([FromQuery] string username)
        {
            User result = await _userService.GetByUsernameAsync(username);
            return Ok(result);
        }
    }
}