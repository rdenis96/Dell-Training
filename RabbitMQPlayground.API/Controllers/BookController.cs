using MassTransit;
using Microsoft.AspNetCore.Mvc;
using RabbitMQPlayground.API.Models;
using RabbitMQPlayground.Domain.Books;
using RabbitMQPlayground.Logic.Books.Services;
using System.Threading.Tasks;

namespace RabbitMQPlayground.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BookController : ControllerBase
    {
        private readonly IBus _bus;
        private readonly IBookService _bookService;

        public BookController(IBus bus, IBookService bookService)
        {
            _bus = bus;
            _bookService = bookService;
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
            Book result = await _bookService.GetByTitleAsync(title);
            return Ok(result);
        }
    }
}