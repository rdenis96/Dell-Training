using Polly;
using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

namespace RabbitMQPlayground.Helpers
{
    public static class HttpHelper
    {
        public static HttpClient CreateClient(IHttpClientFactory httplientFactory)
        {
            var httpClient = httplientFactory.CreateClient();

            return httpClient;
        }

        public static async Task<string> TryGetResponseAsync<TResult>(Func<Task<HttpResponseMessage>> action)
        {
            using (HttpResponseMessage response = await TryExecuteAsync(action))
            {
                using (var responseContent = response.Content)
                {
                    var responseJson = await responseContent.ReadAsStringAsync();
                    if (string.IsNullOrWhiteSpace(responseJson) == false)
                    {
                        return responseJson;
                    }
                }
            }
            return string.Empty;
        }

        private static async Task<HttpResponseMessage> TryExecuteAsync(Func<Task<HttpResponseMessage>> action)
        {
            var retryErrorCode = new HttpStatusCode[] { HttpStatusCode.BadRequest, HttpStatusCode.Unauthorized, HttpStatusCode.Forbidden, HttpStatusCode.InternalServerError };
            var response = await Policy
                // When recieving a ApiException with status code 401 or 500
                .HandleResult<HttpResponseMessage>(message => retryErrorCode.Contains(message.StatusCode))
                // Retry count but execute some code before retrying
                .RetryAsync(3)
                .ExecuteAsync(action);

            response.EnsureSuccessStatusCode();

            return response;
        }
    }
}