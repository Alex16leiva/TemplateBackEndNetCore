using Infraestructura.Core.Exception;
using Newtonsoft.Json;
using System.Net.Http.Headers;

namespace Infraestructura.Core.RestClient
{
    public sealed class HttpRestClient : IRestClient
    {
        private readonly HttpClient _httpClient;


        /// <summary>
        /// Creates a new instance of <see cref="HttpRestClient"/>.
        /// </summary>
        /// <param name="httpClient">The client used internally to consume rest API.</param>
        public HttpRestClient(HttpClient httpClient)
        {
            ThrowIf.Argument.IsNull(httpClient, nameof(httpClient));

            _httpClient = httpClient;
        }

        public async Task<TResponse> GetAsync<TResponse>(string uri) where TResponse : class
        {
            return await SendRequestAsync<TResponse>(uri);
        }

        /// <inheritdoc />
        public async Task<TResponse> PostAsync<TContent, TResponse>(string uri, TContent content)
            where TContent : class
            where TResponse : class
        {
            return await SendRequestAsync<TContent, TResponse>(uri, HttpMethod.Post, content);
        }

        /// <inheritdoc />
        public async Task<TResponse> PutAsync<TContent, TResponse>(string uri, TContent content)
            where TContent : class
            where TResponse : class
        {
            return await SendRequestAsync<TContent, TResponse>(uri, HttpMethod.Put, content);
        }

        /// <inheritdoc />
        public async Task<TResponse> PatchAsync<TContent, TResponse>(string uri, TContent content)
            where TContent : class
            where TResponse : class
        {
            return await SendRequestAsync<TContent, TResponse>(uri, HttpMethod.Put, content);
        }

        /// <inheritdoc />
        public async Task<TResponse> DeleteAsync<TContent, TResponse>(string uri, TContent content)
            where TContent : class
            where TResponse : class
        {
            return await SendRequestAsync<TContent, TResponse>(uri, HttpMethod.Delete, content);
        }

        /// <summary>
        ///
        /// </summary>
        /// <typeparam name="TResponse"></typeparam>
        /// <param name="uri">The Uri the request is sent to.</param>
        /// <returns>The task object of type <see cref="TResponse"/> representing the asynchronous operation.</returns>
        private async Task<TResponse> SendRequestAsync<TResponse>(string uri)
            where TResponse : class
        {
            HttpResponseMessage response = await _httpClient.GetAsync(uri);

            response.EnsureSuccessStatusCode();

            string stringData = response.Content.ReadAsStringAsync().Result;

            return JsonConvert.DeserializeObject<TResponse>(stringData);
        }

        /// <summary>
        ///
        /// </summary>
        /// <typeparam name="TContent"></typeparam>
        /// <typeparam name="TResponse"></typeparam>
        /// <param name="uri">The Uri the request is sent to.</param>
        /// <param name="httpMethod"></param>
        /// <param name="content">The HTTP request content sent to the server.</param>
        /// <remarks>This operation will not block. The returned task object will complete after the whole response (including content) is read.</remarks>
        /// <returns>The task object of type <see cref="TResponse"/> representing the asynchronous operation.</returns>
        private async Task<TResponse> SendRequestAsync<TContent, TResponse>(string uri, HttpMethod httpMethod, TContent content)
                        where TContent : class
            where TResponse : class
        {
            ByteArrayContent byteContent = null;

            if (content != null)
            {
                var json = JsonConvert.SerializeObject(content);

                var buffer = System.Text.Encoding.UTF8.GetBytes(json);
                byteContent = new ByteArrayContent(buffer);
                byteContent.Headers.ContentType = new MediaTypeHeaderValue(@"application/json");
            }

            HttpResponseMessage response = new HttpResponseMessage();

            switch (httpMethod?.ToString()?.ToUpper())
            {
                case "POST":
                    response = await _httpClient.PostAsync(uri, byteContent);
                    break;

                case "PUT":
                    response = await _httpClient.PutAsync(uri, byteContent);
                    break;

                case "DELETE":
                    response = await _httpClient.DeleteAsync(uri);
                    break;

                default:
                    break;
            }

            response.EnsureSuccessStatusCode();

            string stringData = response.Content.ReadAsStringAsync().Result;

            return JsonConvert.DeserializeObject<TResponse>(stringData);
        }
    }
}
