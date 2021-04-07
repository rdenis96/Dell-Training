using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson.Serialization.IdGenerators;
using RabbitMQPlayground.Domain.Common;
using System;

namespace RabbitMQPlayground.Domain.Books
{
    [Serializable]
    public class Book : IBook, IMongoEntity, IEquatable<Book>
    {
        [BsonId(IdGenerator = typeof(ObjectIdGenerator))]
        public ObjectId Id { get; set; }

        public string Date { get; set; }

        public int PagesCount { get; set; }

        public string Title { get; set; }
        public string Category { get; set; }

        public override bool Equals(object obj)
        {
            return Equals(obj as Book);
        }

        public bool Equals(Book other)
        {
            return other != null &&
                   Id.Equals(other.Id) &&
                   Date == other.Date &&
                   PagesCount == other.PagesCount &&
                   Title == other.Title;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Id, Date, PagesCount, Title);
        }
    }
}