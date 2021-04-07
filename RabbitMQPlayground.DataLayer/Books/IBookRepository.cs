using RabbitMQPlayground.DataLayer.Common;
using RabbitMQPlayground.Domain.Books;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace RabbitMQPlayground.DataLayer.Books
{
    public interface IBookRepository : IMongoBaseRepository<Book>
    {
        Task<ICollection<Book>> GetByTitleAsync(string title);
    }
}