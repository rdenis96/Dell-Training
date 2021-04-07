using MongoDB.Bson;

namespace RabbitMQPlayground.Domain.Common
{
    public interface IMongoEntity : IEntity<ObjectId>
    {
    }
}