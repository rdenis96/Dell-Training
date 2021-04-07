using RabbitMQPlayground.Domain.Books;

namespace RabbitMQPlayground.API.Models
{
    public class BookDto : IBook
    {
        public string Date { get; set; }

        public int PagesCount { get; set; }

        public string Title { get; set; }
        public string Category { get; set; }
    }
}