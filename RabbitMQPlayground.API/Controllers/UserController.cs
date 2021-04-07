using MassTransit;
using Microsoft.AspNetCore.Mvc;
using RabbitMQPlayground.API.Models;
using RabbitMQPlayground.Domain.Users;
using RabbitMQPlayground.Logic.Users.Services;
using System.Threading.Tasks;

namespace RabbitMQPlayground.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IBus _bus;
        private readonly IUserService _userService;

        public UserController(IBus bus, IUserService userService)
        {
            _bus = bus;
            _userService = userService;
        }

        [HttpPost("[action]")]
        public async Task<OkResult> Add([FromBody] UserDto user)
        {
            await _bus.Publish<IUser>(user);
            return Ok();
        }

        [HttpPut("[action]")]
        public async Task<OkResult> Update([FromBody] IUserWrapper user)
        {
            await _bus.Publish(user);
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