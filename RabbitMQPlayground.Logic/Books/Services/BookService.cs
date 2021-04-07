using RabbitMQPlayground.Domain.Books;
using System.Threading.Tasks;

namespace RabbitMQPlayground.Logic.Books.Services
{
    public class BookService : IBookService
    {
        public BookService()
        {
        }

        public async Task Add(Book book)
        {
            try
            {
                //await _cache.Set(bookRequest.Title, bookRequest);
            }
            catch
            {
                throw;
            }
        }

        public async Task<Book> GetByTitleAsync(string title)
        {
            Book bookResponse = new Book();

            return bookResponse;
        }
    }
}