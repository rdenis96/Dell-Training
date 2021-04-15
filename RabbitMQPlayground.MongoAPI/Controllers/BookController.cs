using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using RabbitMQPlayground.DataLayer.Books;
using RabbitMQPlayground.Domain.Books;
using System.Threading.Tasks;

namespace RabbitMQPlayground.MongoAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BookController : ControllerBase
    {
        private readonly IBookRepository _bookRepository;
        private readonly ILogger<BookController> _logger;

        public BookController(IBookRepository bookRepository, ILogger<BookController> logger)
        {
            _bookRepository = bookRepository;
            _logger = logger;
        }

        [HttpPost]
        public async Task<IActionResult> Create(Book book)
        {
            var result = await _bookRepository.CreateAsync(book);
            return Ok(result);
        }

        [HttpGet("[action]")]
        [ProducesResponseType(typeof(Book), 200)]
        public async Task<IActionResult> GetByTitle([FromQuery] string title)
        {
            var result = await _bookRepository.GetByTitleAsync(title);
            return Ok(result);
        }
    }
}