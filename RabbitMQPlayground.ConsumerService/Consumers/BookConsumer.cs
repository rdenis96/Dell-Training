using MassTransit;
using Microsoft.Extensions.Logging;
using RabbitMQPlayground.DataLayer.Common;
using RabbitMQPlayground.Domain.Books;
using RabbitMQPlayground.Logic.Books;
using System;
using System.Threading.Tasks;

namespace RabbitMQPlayground.ConsumerService.Consumers
{
    public class BookConsumer : IConsumer<IBook>
    {
        private readonly IBookService _bookService;
        private readonly ILogger<BookConsumer> _logger;

        public BookConsumer(ICompositionRoot compositionRoot, ILogger<BookConsumer> logger)
        {
            _bookService = compositionRoot.GetImplementation<IBookService>();
            _logger = logger;
        }

        public async Task Consume(ConsumeContext<IBook> context)
        {
            var book = new Book
            {
                Title = context.Message.Title,
                PagesCount = context.Message.PagesCount,
                Date = context.Message.Date,
                Category = context.Message.Category
            };

            Console.WriteLine($"Received: book {context.Message.Title} with {context.Message.PagesCount} pages written on {context.Message.Date}");

            try
            {
                await _bookService.Add(book);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
            }
        }
    }
}