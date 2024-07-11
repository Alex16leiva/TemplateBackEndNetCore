namespace Infraestructura.Core.RestClient
{
    /// <summary>
    /// HTTP rest client contract to interact with REST APIS.
    /// </summary>
    public interface IRestClient
    {
        /// <summary>
        /// Sends a GET request to the specified Uri as an asynchronous operation.
        /// </summary>
        /// <typeparam name="TResponse">The response type.</typeparam>
        /// <param name="uri">The Uri the request is sent to.</param>
        /// <remarks>
        /// This operation will not block. The returned task object will complete after the whole response (including content) is read.
        /// </remarks>
        /// <returns>The task object of type <see cref="TResponse"/> representing the asynchronous operation.</returns>
        Task<TResponse> GetAsync<TResponse>(string uri) where TResponse : class;

        /// <summary>
        /// Sends a POST request to the specified Uri as an asynchronous operation.
        /// </summary>
        /// <typeparam name="TContent">The HTTP request content type.</typeparam>
        /// <typeparam name="TResponse">The HTTP request response type.</typeparam>
        /// <param name="uri">The Uri the request is sent to.</param>
        /// <param name="content">The HTTP request content sent to the server.</param>
        /// <remarks>This operation will not block. The returned task object will complete after the whole response (including content) is read.</remarks>
        /// <returns>The task object of type <see cref="TResponse"/> representing the asynchronous operation.</returns>
        Task<TResponse> PostAsync<TContent, TResponse>(string uri, TContent content)
            where TContent : class
            where TResponse : class;

        /// <summary>
        /// Sends a PUT request to the specified Uri as an asynchronous operation.
        /// </summary>
        /// <typeparam name="TContent">The HTTP request content type.</typeparam>
        /// <typeparam name="TResponse">The HTTP request response type.</typeparam>
        /// <param name="uri">The Uri the request is sent to.</param>
        /// <param name="content">The HTTP request content sent to the server.</param>
        /// <remarks>This operation will not block. The returned task object will complete after the whole response (including content) is read.</remarks>
        /// <returns>The task object of type <see cref="TResponse"/> representing the asynchronous operation.</returns>
        Task<TResponse> PutAsync<TContent, TResponse>(string uri, TContent content)
            where TContent : class
            where TResponse : class;

        /// <summary>
        /// Sends a PATCH request to the specified Uri as an asynchronous operation.
        /// </summary>
        /// <typeparam name="TContent">The HTTP request content type.</typeparam>
        /// <typeparam name="TResponse">The HTTP request response type.</typeparam>
        /// <param name="uri">The Uri the request is sent to.</param>
        /// <param name="content">The HTTP request content sent to the server.</param>
        /// <remarks>This operation will not block. The returned task object will complete after the whole response (including content) is read.</remarks>
        /// <returns>The task object of type <see cref="TResponse"/> representing the asynchronous operation.</returns>
        Task<TResponse> PatchAsync<TContent, TResponse>(string uri, TContent content)
            where TContent : class
            where TResponse : class;

        /// <summary>
        /// Sends a DELETE request to the specified Uri as an asynchronous operation.
        /// </summary>
        /// <typeparam name="TContent">The HTTP request content type.</typeparam>
        /// <typeparam name="TResponse">The HTTP request response type.</typeparam>
        /// <param name="uri">The Uri the request is sent to.</param>
        /// <param name="content">The HTTP request content sent to the server.</param>
        /// <remarks>This operation will not block. The returned task object will complete after the whole response (including content) is read.</remarks>
        /// <returns>The task object of type <see cref="TResponse"/> representing the asynchronous operation.</returns>
        Task<TResponse> DeleteAsync<TContent, TResponse>(string uri, TContent content)
            where TContent : class
            where TResponse : class;
    }
}
