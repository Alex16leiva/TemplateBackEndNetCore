using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infraestructura.Core.RestClient
{
    public interface IRestClientFactory
    {
        /// <summary>
        /// Gets a <see cref="IRestClient"/> client.
        /// </summary>
        /// <param name="baseAddress">The base API address to consume.</param>
        /// <returns>The <see cref="IRestClient"/>.</returns>
        IRestClient Create(string baseAddress);
    }
}
