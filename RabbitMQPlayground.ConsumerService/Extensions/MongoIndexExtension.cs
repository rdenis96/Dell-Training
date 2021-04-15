using Microsoft.AspNetCore.Builder;
using MongoDB.Driver;
using RabbitMQPlayground.DataLayer.Contexts;
using RabbitMQPlayground.Domain.Books;
using RabbitMQPlayground.Domain.Users;
using RabbitMQPlayground.Helpers;
using System;
using System.Collections.Generic;

namespace RabbitMQPlayground.ConsumerService.Extensions
{
    public static class MongoIndexExtension
    {
        private static IMongoDatabase _database;

        public static void UseMongoCustomIndexes(this IApplicationBuilder app)
        {
            try
            {
                _database = MongoContext.GetMongoDatabase(AppConfigurationBuilder.Instance.MongoSettings.Database);

                CreateUsersIndexes();
                CreateBooksIndexes();
            }
            catch (Exception ex)
            {
                //Handle if any exception would be thrown to make sure the Startup will not stop.
                Console.WriteLine(ex.ToString());
            }
        }

        private static void CreateUsersIndexes()
        {
            var usersCollection = _database.GetCollection<User>("Users");

            var usersBuilder = Builders<User>.IndexKeys;
            var usersIndex = new CreateIndexModel<User>(usersBuilder.Ascending(x => x.Username), new CreateIndexOptions() { Unique = true });
            usersCollection.Indexes.CreateOneAsync(usersIndex).Wait();
        }

        private static void CreateBooksIndexes()
        {
            var booksCollection = _database.GetCollection<Book>("Books");
            var booksBuilder = Builders<Book>.IndexKeys;
            var booksIndexes = new List<CreateIndexModel<Book>>
                {
                    new CreateIndexModel<Book>(booksBuilder.Ascending(x => x.Title)),
                    new CreateIndexModel<Book>(booksBuilder.Ascending(x => x.Category))
                };

            booksCollection.Indexes.CreateManyAsync(booksIndexes).Wait();
        }
    }
}