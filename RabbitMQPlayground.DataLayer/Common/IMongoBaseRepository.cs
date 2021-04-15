using RabbitMQPlayground.Domain.Common;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace RabbitMQPlayground.DataLayer.Common
{
    public interface IMongoBaseRepository<TEntity> where TEntity : class, IMongoEntity, new()
    {
        Task<TEntity> CreateAsync(TEntity obj);

        Task<ICollection<TEntity>> GetAllAsync();

        Task<ICollection<TEntity>> GetAllInBatchesAsync(int batchNumber, int batchSize);

        Task<long> GetDocumentCountAsync();

        Task<TEntity> GetByIdAsync(string objId);

        Task<TEntity> UpdateAsync(TEntity obj);

        Task UpdateManyAsync(List<TEntity> objList);

        Task<bool> DeleteAsync(TEntity obj);
    }
}