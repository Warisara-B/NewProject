using System;

namespace Plexus.Utility.Proxy.src
{
    public class HttpClientProxy : IHttpClientProxy
    {
        private readonly HttpClient _httpClient;

        public HttpClientProxy(IHttpClientFactory httpClientFactory)
        {
            _httpClient = httpClientFactory.CreateClient();
        }

        public async Task<HttpResponseMessage> PostAsync(string endpoint, IDictionary<string, string> headers, HttpContent content)
        {
            var requestMessage = GenerateHttpRequestMessage(HttpMethod.Post, endpoint, headers, content);
            return await _httpClient.SendAsync(requestMessage, CancellationToken.None);
        }

        public async Task<HttpResponseMessage> GetAsync(string endpoint, IDictionary<string, string> headers)
        {
            var requestMessage = GenerateHttpRequestMessage(HttpMethod.Get, endpoint, headers);
            return await _httpClient.SendAsync(requestMessage, CancellationToken.None);
        }

        public async Task<HttpResponseMessage> PutAsync(string endpoint, IDictionary<string, string> headers, HttpContent content)
        {
            var requestMessage = GenerateHttpRequestMessage(HttpMethod.Put, endpoint, headers, content);
            return await _httpClient.SendAsync(requestMessage, CancellationToken.None);
        }

        public async Task<HttpResponseMessage> DeleteAsync(string endpoint, IDictionary<string, string> headers, HttpContent content)
        {
            var requestMessage = GenerateHttpRequestMessage(HttpMethod.Delete, endpoint, headers, content);
            return await _httpClient.SendAsync(requestMessage, CancellationToken.None);
        }

        private static HttpRequestMessage GenerateHttpRequestMessage(
            HttpMethod method, string endpoint,
            IDictionary<string, string> headers, HttpContent content = null)
        {
            var requestMessage = new HttpRequestMessage(method, endpoint);

            if (headers is not null)
            {
                foreach (var header in headers)
                {
                    requestMessage.Headers.Add(header.Key, header.Value);
                }
            }

            requestMessage.Content = content;

            return requestMessage;
        }
    }
}

