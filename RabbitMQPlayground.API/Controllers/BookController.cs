using MassTransit;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using RabbitMQPlayground.API.Models;
using RabbitMQPlayground.DataLayer.Common;
using RabbitMQPlayground.Domain.Books;
using RabbitMQPlayground.Logic.Books;
using System.Threading.Tasks;

namespace RabbitMQPlayground.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BookController : ControllerBase
    {
        private readonly IBus _bus;
        private readonly IBookService _bookService;
        private readonly ILogger<BookController> _logger;

        public BookController(IBus bus, ICompositionRoot compositionRoot, ILogger<BookController> logger)
        {
            _bus = bus;
            _bookService = compositionRoot.GetImplementation<IBookService>();
            _logger = logger;
        }

        [HttpPost]
        public async Task<OkResult> Add(BookDto book)
        {
            await _bus.Publish<IBook>(book);
            await _bus.Publish(book, context =>
            {
                context.SetRoutingKey(book.Category);
            });
            return Ok();
        }

        [HttpGet("[action]")]
        [ProducesResponseType(typeof(Book), 200)]
        public async Task<ActionResult> GetBookByTitle([FromQuery] string title)
        {
            var result = await _bookService.GetByTitleAsync(title);
            return Ok(result);
        }
    }
}