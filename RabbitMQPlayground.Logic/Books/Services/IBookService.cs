using RabbitMQPlayground.Domain.Books;
using System.Threading.Tasks;

namespace RabbitMQPlayground.Logic.Books.Services
{
    public interface IBookService
    {
        Task<Book> GetByTitleAsync(string title);

        Task Add(Book book);
    }
}