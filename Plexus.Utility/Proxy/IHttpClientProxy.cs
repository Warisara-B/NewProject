
namespace Plexus.Utility.Proxy
{
    public interface IHttpClientProxy
    {
        /// <summary>
        /// Handle call http get according to the parameter
        /// </summary>
        /// <param name="endpoint"></param>
        /// <param name="headers"></param>
        /// <returns></returns>
        Task<HttpResponseMessage> GetAsync(string endpoint, IDictionary<string, string> headers);

        /// <summary>
        /// Handle call http post according to the parameter
        /// </summary>
        /// <param name="endpoint"></param>
        /// <param name="headers"></param>
        /// <param name="content"></param>
        /// <returns></returns>
        Task<HttpResponseMessage> PostAsync(string endpoint, IDictionary<string, string> headers, HttpContent content);

        /// <summary>
        /// Handle call http put according to the parameter
        /// </summary>
        /// <param name="endpoint"></param>
        /// <param name="headers"></param>
        /// <param name="content"></param>
        /// <returns></returns>
        Task<HttpResponseMessage> PutAsync(string endpoint, IDictionary<string, string> headers, HttpContent content);

        /// <summary>
        /// Handle call http delete according to the parameter
        /// </summary>
        /// <param name="endpoint"></param>
        /// <param name="headers"></param>
        /// <param name="content"></param>
        /// <returns></returns>
        Task<HttpResponseMessage> DeleteAsync(string endpoint, IDictionary<string, string> headers, HttpContent content);
    }
}