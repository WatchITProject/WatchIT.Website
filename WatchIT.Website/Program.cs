using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using System.Text.Json;
using System.Text.Json.Serialization;
using WatchIT.Website.Authentication;
using WatchIT.Website.Services;
using WatchIT.Website.Services.Accounts;
using WatchIT.Website.Authentication;
using WatchIT.Website.Services.Website;
using WatchIT.Website.Services.Website.AuthBackground;

namespace WatchIT.Website
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebAssemblyHostBuilder.CreateDefault(args);
            builder.RootComponents.Add<App>("#app");
            builder.RootComponents.Add<HeadOutlet>("head::after");

            // Storage
            builder.Services.AddBlazoredLocalStorageAsSingleton(config =>
            {
                config.JsonSerializerOptions.DictionaryKeyPolicy = JsonNamingPolicy.CamelCase;
                config.JsonSerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull;
                config.JsonSerializerOptions.IgnoreReadOnlyProperties = true;
                config.JsonSerializerOptions.PropertyNameCaseInsensitive = true;
                config.JsonSerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
                config.JsonSerializerOptions.ReadCommentHandling = JsonCommentHandling.Skip;
                config.JsonSerializerOptions.WriteIndented = false;
            });

            // Clients
            builder.Services.AddSingleton<HttpClient>();
            builder.Services.AddSingleton<ApiClient>();

            // Configuration
            builder.Services.AddSingleton<ApiConfiguration>();
            builder.Services.AddSingleton<AccountsConfiguration>();
            builder.Services.AddSingleton<WebsiteConfiguration>();
            builder.Services.AddSingleton<WebsiteAuthBackgroundConfiguration>();

            // Services
            builder.Services.AddSingleton<IAuthenticationService, AuthenticationService>();
            builder.Services.AddSingleton<IAccountsService, AccountsService>();
            builder.Services.AddSingleton<IWebsiteAuthBackgroundService, WebsiteAuthBackgroundService>();

            // Auth
            builder.Services.AddAuthorizationCore();
            builder.Services.AddSingleton<AuthenticationStateProvider, JWTAuthenticationStateProvider>();

            await builder.Build().RunAsync();
        }
    }
}
