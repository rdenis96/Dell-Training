using RabbitMQPlayground.Domain.Books;
using RabbitMQPlayground.Logic.Common;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace RabbitMQPlayground.Logic.Books
{
    public interface IBookService : IBaseService<Book>
    {
        Task<ICollection<Book>> GetByTitleAsync(string title);
    }
}