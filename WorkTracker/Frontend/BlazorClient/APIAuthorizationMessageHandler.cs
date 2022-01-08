using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.WebAssembly.Authentication;

namespace BlazorClient
{
    public class APIAuthorizationMessageHandler : AuthorizationMessageHandler
    {
        public APIAuthorizationMessageHandler(IAccessTokenProvider provider, NavigationManager navigation, IConfiguration config) : base(provider, navigation)
        {
            ConfigureHandler(
                authorizedUrls: new[] { config.GetValue<string>("API:Url") },
                scopes: new[] { config.GetValue<string>("AzureAd:Scope") }
                );
        }
    }
}
