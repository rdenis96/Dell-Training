using Newtonsoft.Json;
using RabbitMQPlayground.Domain.Books;
using RabbitMQPlayground.Helpers;
using RabbitMQPlayground.Logic.Common;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.Net.Http;
using System.Net.Mime;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace RabbitMQPlayground.Logic.Books
{
    public class BookService : BaseApiService<Book>, IBookService
    {
        private readonly HttpClient _client;

        public BookService(IHttpClientFactory httpClientFactory) : base()
        {
            _client = HttpHelper.CreateClient(httpClientFactory);
        }

        public async Task<Book> Add(Book book)
        {
            try
            {
                var url = new Uri(Path.Combine(BasePath, "Create"));
                var response = await HttpHelper.TryGetResponseAsync<string>(async () =>
                {
                    using (var requestMessage = new HttpRequestMessage(HttpMethod.Post, url))
                    {
                        requestMessage.Content = new StringContent(JsonConvert.SerializeObject(book), Encoding.UTF8, MediaTypeNames.Application.Json);
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

        public async Task<Book> Update(Book book)
        {
            try
            {
                var url = new Uri(Path.Combine(BasePath, "Update"));
                var response = await HttpHelper.TryGetResponseAsync<string>(async () =>
                {
                    using (var requestMessage = new HttpRequestMessage(HttpMethod.Put, url))
                    {
                        requestMessage.Content = new StringContent(JsonConvert.SerializeObject(book), Encoding.UTF8, MediaTypeNames.Application.Json);
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

        public async Task<ICollection<Book>> GetByTitleAsync(string title)
        {
            var url = new Uri(Path.Combine(BasePath, "GetByTitle"));
            var response = await HttpHelper.TryGetResponseAsync<string>(async () =>
            {
                NameValueCollection queryString = HttpUtility.ParseQueryString(string.Empty);
                queryString.Add(nameof(title), title);

                var finalUri = new Uri($"{url}?{queryString}");

                using (var requestMessage = new HttpRequestMessage(HttpMethod.Get, finalUri))
                {
                    return await _client.SendAsync(requestMessage);
                };
            });

            var result = GetCollectionFromJson(response);
            return result;
        }
    }
}