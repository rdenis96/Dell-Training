using System.Threading.Tasks;

namespace RabbitMQPlayground.Logic.Common
{
    public interface IBaseService<T>
    {
        Task<T> Add(T user);

        Task<T> Update(T user);
    }
}