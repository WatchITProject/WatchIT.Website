using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WatchIT.Common;
using WatchIT.Common.Accounts.Request;
using WatchIT.Common.Accounts.Response;
using WatchIT.Website.Services;

namespace WatchIT.Website.Services.Accounts
{
    public interface IAccountsService
    {
        Task<ApiResponse> Register(RegisterRequest data);
        Task<ApiResponse<AuthenticateResponse>> Authenticate(AuthenticateRequest data);
        Task<ApiResponse<AuthenticateResponse>> AuthenticateRefresh();
    }

    public class AccountsService : IAccountsService
    {
        #region FIELDS

        private readonly ApiClient _apiClient;
        private readonly AccountsConfiguration _configuration;

        #endregion



        #region CONSTRUCTORS

        public AccountsService(ApiClient apiClient, AccountsConfiguration configuration)
        {
            _apiClient = apiClient;
            _configuration = configuration;
        }

        #endregion



        #region METHODS

        public async Task<ApiResponse> Register(RegisterRequest data)
        {
            return await _apiClient.SendAsync<RegisterRequest>(ApiMethodType.POST, _configuration.AccountsRegister, data);
        }

        public async Task<ApiResponse<AuthenticateResponse>> Authenticate(AuthenticateRequest data)
        {
            return await _apiClient.SendAsync<AuthenticateResponse, AuthenticateRequest>(ApiMethodType.POST, _configuration.AccountsAuthenticate, data);
        }

        public async Task<ApiResponse<AuthenticateResponse>> AuthenticateRefresh()
        {
            return await _apiClient.SendAsync<AuthenticateResponse>(ApiMethodType.POST, _configuration.AccountsAuthenticateRefresh);
        }

        #endregion
    }
}
