using MongoDB.Bson;
using MongoDB.Driver;
using RabbitMQPlayground.DataLayer.Contexts;
using RabbitMQPlayground.Domain.Common;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RabbitMQPlayground.DataLayer.Common
{
    public abstract class MongoBaseRepository<TEntity> : IMongoBaseRepository<TEntity> where TEntity : class, IMongoEntity, new()
    {
        protected IMongoCollection<TEntity> _collection;

        public MongoBaseRepository(string databaseName, string collectionName)
        {
            _collection = MongoContext.GetMongoDatabase(databaseName).GetCollection<TEntity>(collectionName);
        }

        public async Task<TEntity> CreateAsync(TEntity obj)
        {
            await _collection.InsertOneAsync(obj);
            if (obj.Id == string.Empty)
            {
                return null;
            }
            return obj;
        }

        public async Task<ICollection<TEntity>> GetAllAsync()
        {
            var taskResult = await _collection.FindAsync(_ => true);
            var result = await taskResult.ToListAsync();
            return result;
        }

        public async Task<ICollection<TEntity>> GetAllInBatchesAsync(int batchNumber, int batchSize)
        {
            var result = await _collection.Find(new BsonDocument()).Skip(batchNumber * batchSize).Limit(batchSize).ToListAsync();

            return result;
        }

        protected IAggregateFluent<TEntity> GetAggregateQuery(string hint = null)
        {
            AggregateOptions aggregateOptions = new AggregateOptions { AllowDiskUse = true, Hint = hint };
            IAggregateFluent<TEntity> query = _collection.Aggregate(aggregateOptions);
            return query;
        }

        public async Task<long> GetDocumentCountAsync()
        {
            var result = await _collection.CountDocumentsAsync(new BsonDocument());

            return result;
        }

        public async Task<TEntity> GetByIdAsync(string objId)
        {
            var taskResult = await _collection.FindAsync(x => x.Id == objId);
            var result = taskResult.FirstOrDefault();
            return result;
        }

        public async Task<TEntity> UpdateAsync(TEntity obj)
        {
            await _collection.ReplaceOneAsync(x => x.Id == obj.Id, obj);
            var taskResult = await _collection.FindAsync(x => x.Id == obj.Id);
            var result = taskResult.FirstOrDefault();
            return result;
        }

        public async Task UpdateManyAsync(List<TEntity> objList)
        {
            List<WriteModel<TEntity>> bulkOps = new List<WriteModel<TEntity>>();
            foreach (var obj in objList)
            {
                if (obj.Id == BsonObjectId.Empty)
                {
                    var insertOne = new InsertOneModel<TEntity>(obj);
                    bulkOps.Add(insertOne);
                }
                else
                {
                    var upsertOne = new ReplaceOneModel<TEntity>(Builders<TEntity>.Filter.Where(x => x.Id == obj.Id), obj) { IsUpsert = true };
                    bulkOps.Add(upsertOne);
                }
            }

            if (bulkOps.Count > 0)
            {
                await _collection.BulkWriteAsync(bulkOps);
            }
        }

        public async Task<bool> DeleteAsync(TEntity obj)
        {
            var result = await _collection.DeleteOneAsync(x => x.Id == obj.Id);
            return result.IsAcknowledged;
        }
    }
}