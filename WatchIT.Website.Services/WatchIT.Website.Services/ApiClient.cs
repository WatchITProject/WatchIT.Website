using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using WatchIT.Common;

namespace WatchIT.Website.Services
{
    public class ApiClient
    {
        #region FIELDS

        private readonly HttpClient _httpClient;

        #endregion



        #region CONSTRUCTORS

        public ApiClient(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        #endregion



        #region PUBLIC METHODS

        public async Task<ApiResponse<TResponse>> SendAsync<TResponse>(ApiMethodType type, string url)
        {
            return await SendRequestAsync<ApiResponse<TResponse>>(type, url, null);
        }

        public async Task<ApiResponse> SendAsync(ApiMethodType type, string url)
        {
            return await SendRequestAsync<ApiResponse>(type, url, null);
        }

        public async Task<ApiResponse<TResponse>> SendAsync<TResponse, TBody>(ApiMethodType type, string url, TBody body)
        {
            HttpContent content = PrepareBody(body);

            return await SendRequestAsync<ApiResponse<TResponse>>(type, url, content);
        }

        public async Task<ApiResponse> SendAsync<TBody>(ApiMethodType type, string url, TBody body)
        {
            HttpContent content = PrepareBody(body);

            return await SendRequestAsync<ApiResponse>(type, url, content);
        }

        #endregion



        #region PRIVATE METHODS

        private HttpContent PrepareBody<T>(T body)
        {
            string json = JsonConvert.SerializeObject(body);

            HttpContent content = new StringContent(json);
            content.Headers.ContentType.MediaType = "application/json";

            return content;
        }

        private async Task<T> SendRequestAsync<T>(ApiMethodType type, string url, HttpContent? content)
        {
            HttpResponseMessage response = type switch
            {
                ApiMethodType.GET => await _httpClient.GetAsync(url),
                ApiMethodType.POST => await _httpClient.PostAsync(url, content),
                ApiMethodType.PUT => await _httpClient.PutAsync(url, content),
                ApiMethodType.DELETE => await _httpClient.DeleteAsync(url),
                _ => throw new NotImplementedException()
            };

            string responseBodyString = await response.Content.ReadAsStringAsync();

            T? responseBodyObject = JsonConvert.DeserializeObject<T>(responseBodyString);

            if (responseBodyObject is null)
            {
                throw new Exception($"Wrong response type. Response: {responseBodyString}");
            }

            return responseBodyObject;
        }

        #endregion
    }
}
