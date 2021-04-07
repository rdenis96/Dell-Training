using MongoDB.Bson.Serialization.Conventions;
using MongoDB.Driver;
using RabbitMQPlayground.Helpers;
using System;

namespace RabbitMQPlayground.DataLayer.Contexts
{
    public static class MongoContext
    {
        static MongoContext()
        {
            MappingEntitiesConfiguration();
        }

        public static IMongoDatabase GetMongoDatabase(string databaseName)
        {
            if (string.IsNullOrWhiteSpace(databaseName) == true)
            {
                throw new Exception("Mongo Database name is not valid!");
            }
            string connectionString = AppConfigurationBuilder.Instance.MongoSettings?.ConnectionString;
            var client = new MongoClient(connectionString);
            var database = client.GetDatabase(databaseName);
            return database;
        }

        private static void MappingEntitiesConfiguration()
        {
            var conventionPack = new ConventionPack();
            conventionPack.Add(new IgnoreExtraElementsConvention(true));

            ConventionRegistry.Register("customConventions", conventionPack, type => true);
        }
    }
}