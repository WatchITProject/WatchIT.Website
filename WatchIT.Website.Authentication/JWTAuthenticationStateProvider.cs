using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components.Authorization;
using System;
using System.Buffers.Text;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using WatchIT.Common;
using WatchIT.Common.Accounts.Response;
using WatchIT.Website.Authentication;
using WatchIT.Website.Services.Accounts;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace WatchIT.Website.Authentication
{
    public class JWTAuthenticationStateProvider : AuthenticationStateProvider
    {
        #region FIELDS

        private readonly IAuthenticationService _authenticationService;
        private readonly IAccountsService _accountsService;

        private readonly HttpClient _httpClient;

        #endregion



        #region CONSTRUCTORS

        public JWTAuthenticationStateProvider(IAuthenticationService authenticationService, HttpClient httpClient, IAccountsService accountsService)
        {
            _authenticationService = authenticationService;
            _httpClient = httpClient;
            _accountsService = accountsService;
        }

        #endregion



        #region PUBLIC METHODS

        public override async Task<AuthenticationState> GetAuthenticationStateAsync()
        {
            _httpClient.DefaultRequestHeaders.Authorization = null;
            AuthenticationState state = new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity()));

            Task<string> accessTokenTask = _authenticationService.GetAccessToken();
            Task<string> refreshTokenTask = _authenticationService.GetRefreshToken();

            await Task.WhenAll(accessTokenTask, refreshTokenTask);

            string accessToken = accessTokenTask.Result;
            string refreshToken = refreshTokenTask.Result;
            bool refreshed = false;

            if (string.IsNullOrWhiteSpace(accessToken))
            {
                if (string.IsNullOrWhiteSpace(refreshToken))
                {
                    return state;
                }

                string? accessTokenNew = await Refresh(refreshToken);

                if (accessTokenNew is null)
                {
                    return state;
                }

                accessToken = accessTokenNew;
                refreshed = true;
            }

            IEnumerable<Claim> claims = ParseClaimsFromToken(accessToken);

            Claim? expClaim = claims.FirstOrDefault(c => c.Type == "exp");

            if (expClaim is not null && DateTimeOffset.FromUnixTimeSeconds(long.Parse(expClaim.Value)) > DateTime.UtcNow)
            {
                if (refreshed)
                {
                    return state;
                }
            }
            else
            {
                if (string.IsNullOrWhiteSpace(refreshToken))
                {
                    return state;
                }

                string? accessTokenNew = await Refresh(refreshToken);

                if (accessTokenNew is null)
                {
                    return state;
                }

                accessToken = accessTokenNew;
            }

            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken.Replace("\"", ""));
            return new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity(claims)));
        }

        #endregion



        #region PRIVATE METHODS

        private async Task<string?> Refresh(string refreshToken)
        {
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("refresh", refreshToken.Replace("\"", ""));

            ApiResponse<AuthenticateResponse> response = await _accountsService.AuthenticateRefresh();

            _httpClient.DefaultRequestHeaders.Authorization = null;

            if (!response.Success)
            {
                return null;
            }

            await _authenticationService.SaveAuthenticationData(response.Data);
            return response.Data.AccessToken;
        }

        private static IEnumerable<Claim> ParseClaimsFromToken(string token)
        {
            string payload = token.Split('.')[1];

            switch (payload.Length % 4)
            {
                case 2: payload += "=="; break;
                case 3: payload += "="; break;
            }

            byte[] jsonBytes = Convert.FromBase64String(payload);
            Dictionary<string, object>? keyValuePairs = JsonSerializer.Deserialize<Dictionary<string, object>>(jsonBytes);

            if (keyValuePairs is null)
            {
                throw new Exception("Incorrect token");
            }

            return keyValuePairs.Select(kvp => new Claim(kvp.Key, kvp.Value.ToString()));
        }

        #endregion
    }
}
