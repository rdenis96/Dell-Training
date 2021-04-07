using MassTransit;
using RabbitMQPlayground.Domain.Books;
using RabbitMQPlayground.Logic.Books.Services;
using System;
using System.Threading.Tasks;

namespace RabbitMQPlayground.Logic.Services.Books.Consumers
{
    public class BookConsumer : IConsumer<IBook>
    {
        private readonly IBookService _bookService;

        public BookConsumer(IBookService bookService)
        {
            _bookService = bookService;
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

            await _bookService.Add(book);
        }
    }
}