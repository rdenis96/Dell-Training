namespace RabbitMQPlayground.DataLayer.Common
{
    public interface ICompositionRoot
    {
        T GetImplementation<T>();

        T GetImplementation<T>(string name);
    }
}