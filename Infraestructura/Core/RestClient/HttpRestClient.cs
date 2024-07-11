using Infraestructura.Core.Exception;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infraestructura.Core.RestClient
{
    public sealed class HttpRestClient
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
    }
}
