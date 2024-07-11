using Infraestructura.Core.Exception;
using System.Net.Http.Headers;

namespace Infraestructura.Core.RestClient
{
    public sealed class HttpRestClientFactory : IRestClientFactory
    {
        private readonly Dictionary<string, HttpClient> _httpClients = new Dictionary<string, HttpClient>();

        public IRestClient Create(string baseAddress)
        {
            ThrowIf.Argument.IsNullOrWhiteSpace(baseAddress, nameof(baseAddress));

            HttpClient httpClient;

            if (_httpClients.ContainsKey(baseAddress) && _httpClients[baseAddress] != null)
            {
                httpClient = _httpClients[baseAddress];
            }
            else
            {
                httpClient = new HttpClient();
                httpClient.BaseAddress = new Uri(baseAddress);
                httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue(@"application/json"));

                if (_httpClients.ContainsKey(baseAddress))
                {
                    _httpClients.Remove(baseAddress);
                }

                _httpClients.Add(baseAddress, httpClient);
            }

            return new HttpRestClient(httpClient);
        }
    }
}
