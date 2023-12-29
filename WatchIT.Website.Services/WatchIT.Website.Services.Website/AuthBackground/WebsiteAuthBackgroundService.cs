using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WatchIT.Common.Accounts.Request;
using WatchIT.Common.Accounts.Response;
using WatchIT.Common;
using WatchIT.Common.Website.AuthBackground.Response;
using WatchIT.Common.Website.AuthBackground.Request;

namespace WatchIT.Website.Services.Website.AuthBackground
{
    public interface IWebsiteAuthBackgroundService
    {
        Task<ApiResponse<short>> AddAuthBackground(AuthBackgroundPostPutRequest data);
        Task<ApiResponse> DeleteAuthBackground(short id, AuthBackgroundPostPutRequest data);
        Task<ApiResponse<AuthBackgroundResponse>> GetAuthBackground(short id);
        Task<ApiResponse<IEnumerable<AuthBackgroundResponse>>> GetAuthBackgrounds();
        Task<ApiResponse<AuthBackgroundResponse>> GetRandomAuthBackground();
        Task<ApiResponse> ModifyAuthBackground(short id, AuthBackgroundPostPutRequest data);
    }

    public class WebsiteAuthBackgroundService : IWebsiteAuthBackgroundService
    {
        #region FIELDS

        private readonly ApiClient _apiClient;
        private readonly WebsiteAuthBackgroundConfiguration _configuration;

        #endregion



        #region CONSTRUCTORS

        public WebsiteAuthBackgroundService(ApiClient apiClient, WebsiteAuthBackgroundConfiguration configuration)
        {
            _apiClient = apiClient;
            _configuration = configuration;
        }

        #endregion



        #region METHODS

        public async Task<ApiResponse<AuthBackgroundResponse>> GetRandomAuthBackground()
        {
            return await _apiClient.SendAsync<AuthBackgroundResponse>(ApiMethodType.GET, _configuration.WebsiteAuthBackgroundGetRandom);
        }

        public async Task<ApiResponse<AuthBackgroundResponse>> GetAuthBackground(short id)
        {
            return await _apiClient.SendAsync<AuthBackgroundResponse>(ApiMethodType.GET, string.Format(_configuration.WebsiteAuthBackgroundGet, id));
        }

        public async Task<ApiResponse<IEnumerable<AuthBackgroundResponse>>> GetAuthBackgrounds()
        {
            return await _apiClient.SendAsync<IEnumerable<AuthBackgroundResponse>>(ApiMethodType.GET, _configuration.WebsiteAuthBackgroundGetAll);
        }

        public async Task<ApiResponse<short>> AddAuthBackground(AuthBackgroundPostPutRequest data)
        {
            return await _apiClient.SendAsync<short, AuthBackgroundPostPutRequest>(ApiMethodType.POST, _configuration.WebsiteAuthBackgroundAdd, data);
        }

        public async Task<ApiResponse> ModifyAuthBackground(short id, AuthBackgroundPostPutRequest data)
        {
            return await _apiClient.SendAsync(ApiMethodType.PUT, string.Format(_configuration.WebsiteAuthBackgroundModify, id), data);
        }

        public async Task<ApiResponse> DeleteAuthBackground(short id, AuthBackgroundPostPutRequest data)
        {
            return await _apiClient.SendAsync(ApiMethodType.DELETE, string.Format(_configuration.WebsiteAuthBackgroundDelete, id), data);
        }

        #endregion
    }
}
