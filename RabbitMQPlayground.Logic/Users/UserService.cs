using Newtonsoft.Json;
using RabbitMQPlayground.Domain.Users;
using RabbitMQPlayground.Helpers;
using RabbitMQPlayground.Logic.Common;
using System;
using System.Collections.Specialized;
using System.IO;
using System.Net.Http;
using System.Net.Mime;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace RabbitMQPlayground.Logic.Users
{
    public class UserService : BaseApiService<User>, IUserService
    {
        private readonly HttpClient _client;

        public UserService(IHttpClientFactory httpClientFactory) : base()
        {
            _client = HttpHelper.CreateClient(httpClientFactory);
        }

        public async Task<User> Add(User user)
        {
            try
            {
                var url = new Uri(Path.Combine(BasePath, "Create"));
                var response = await HttpHelper.TryGetResponseAsync<string>(async () =>
                {
                    using (var requestMessage = new HttpRequestMessage(HttpMethod.Post, url))
                    {
                        requestMessage.Content = new StringContent(JsonConvert.SerializeObject(user), Encoding.UTF8, MediaTypeNames.Application.Json);
                        return await _client.SendAsync(requestMessage);
                    };
                });

                var result = GetObjectFromJson(response);
                return result;
            }
            catch
            {
                throw;
            }
        }

        public async Task<User> Update(User user)
        {
            try
            {
                var url = new Uri(Path.Combine(BasePath, "Update"));
                var response = await HttpHelper.TryGetResponseAsync<string>(async () =>
                {
                    using (var requestMessage = new HttpRequestMessage(HttpMethod.Put, url))
                    {
                        requestMessage.Content = new StringContent(JsonConvert.SerializeObject(user), Encoding.UTF8, MediaTypeNames.Application.Json);
                        return await _client.SendAsync(requestMessage);
                    };
                });

                var result = GetObjectFromJson(response);
                return result;
            }
            catch
            {
                throw;
            }
        }

        public async Task<User> GetByUsernameAsync(string username)
        {
            var url = new Uri(Path.Combine(BasePath, "GetByUsername"));
            var response = await HttpHelper.TryGetResponseAsync<string>(async () =>
            {
                NameValueCollection queryString = HttpUtility.ParseQueryString(string.Empty);
                queryString.Add(nameof(username), username);

                var finalUri = new Uri($"{url}?{queryString}");

                using (var requestMessage = new HttpRequestMessage(HttpMethod.Get, finalUri))
                {
                    return await _client.SendAsync(requestMessage);
                };
            });

            var result = GetObjectFromJson(response);
            return result;
        }
    }
}