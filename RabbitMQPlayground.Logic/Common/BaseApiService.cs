using Newtonsoft.Json;
using RabbitMQPlayground.Helpers;
using System.Collections.Generic;
using System.IO;

namespace RabbitMQPlayground.Logic.Common
{
    public abstract class BaseApiService<T> where T : class
    {
        protected string BasePath { get; set; }

        public BaseApiService()
        {
            BasePath = Path.Combine(AppConfigurationBuilder.Instance.RepositoryUrl, typeof(T).Name);
        }

        protected T GetObjectFromJson(string json)
        {
            var result = JsonConvert.DeserializeObject<T>(json);
            return result;
        }

        protected ICollection<T> GetCollectionFromJson(string json)
        {
            var result = JsonConvert.DeserializeObject<ICollection<T>>(json);
            return result;
        }
    }
}