using Blazored.LocalStorage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WatchIT.Common.Accounts.Response;
using WatchIT.Website.Authentication;

namespace WatchIT.Website.Authentication
{
    public interface IAuthenticationService
    {
        Task<string> GetAccessToken();
        Task<string> GetRefreshToken();
        Task RemoveAccessToken();
        Task RemoveAuthenticationData();
        Task RemoveRefreshToken();
        Task SaveAccessToken(string accessToken);
        Task SaveAuthenticationData(AuthenticateResponse authenticateResponse);
        Task SaveRefreshToken(string refreshToken);
    }



    public class AuthenticationService : IAuthenticationService
    {
        #region FIELDS

        private readonly ILocalStorageService _localStorageService;

        #endregion



        #region CONSTRUCTORS

        public AuthenticationService(ILocalStorageService localStorageService)
        {
            _localStorageService = localStorageService;
        }

        #endregion



        #region PUBLIC METHODS

        public async Task<string> GetAccessToken() => await _localStorageService.GetItemAsync<string>(AuthenticationConfiguration.ACCESS_TOKEN_STORAGE_KEY);

        public async Task<string> GetRefreshToken() => await _localStorageService.GetItemAsync<string>(AuthenticationConfiguration.REFRESH_TOKEN_STORAGE_KEY);

        public async Task SaveAuthenticationData(AuthenticateResponse authenticateResponse) => await Task.WhenAll(SaveAccessToken(authenticateResponse.AccessToken), SaveRefreshToken(authenticateResponse.RefreshToken));

        public async Task SaveAccessToken(string accessToken) => await _localStorageService.SetItemAsync(AuthenticationConfiguration.ACCESS_TOKEN_STORAGE_KEY, accessToken);

        public async Task SaveRefreshToken(string refreshToken) => await _localStorageService.SetItemAsync(AuthenticationConfiguration.REFRESH_TOKEN_STORAGE_KEY, refreshToken);

        public async Task RemoveAuthenticationData() => await Task.WhenAll(RemoveAccessToken(), RemoveRefreshToken());

        public async Task RemoveAccessToken() => await _localStorageService.RemoveItemAsync(AuthenticationConfiguration.ACCESS_TOKEN_STORAGE_KEY);

        public async Task RemoveRefreshToken() => await _localStorageService.RemoveItemAsync(AuthenticationConfiguration.REFRESH_TOKEN_STORAGE_KEY);

        #endregion
    }
}
