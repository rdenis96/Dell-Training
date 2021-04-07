using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson.Serialization.IdGenerators;
using RabbitMQPlayground.Domain.Common;
using System;

namespace RabbitMQPlayground.Domain.Users
{
    public class User : IUser, IMongoEntity, IEquatable<User>
    {
        [BsonId(IdGenerator = typeof(ObjectIdGenerator))]
        public ObjectId Id { get; set; }

        public string Username { get; set; }
        public string Password { get; set; }

        public override bool Equals(object obj)
        {
            return Equals(obj as User);
        }

        public bool Equals(User other)
        {
            return other != null &&
                   Id.Equals(other.Id) &&
                   Username == other.Username &&
                   Password == other.Password;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Id, Username, Password);
        }
    }
}