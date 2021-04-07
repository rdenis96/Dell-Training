using MongoDB.Driver;
using RabbitMQPlayground.DataLayer.Common;
using RabbitMQPlayground.Domain.Books;
using RabbitMQPlayground.Helpers;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace RabbitMQPlayground.DataLayer.Books
{
    public class BookRepository : MongoBaseRepository<Book>, IBookRepository
    {
        public BookRepository() : base(AppConfigurationBuilder.Instance.MongoSettings.Database, "Books")
        {
        }

        public async Task<ICollection<Book>> GetByTitleAsync(string title)
        {
            var taskResult = await _collection.FindAsync(x => x.Title == title);
            var result = await taskResult.ToListAsync();
            return result;
        }
    }
}