using ApiAccess.Services;
using BlazorClient;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Authentication;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Radzen;

var builder = WebAssemblyHostBuilder.CreateDefault(args);

builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddScoped<APIAuthorizationMessageHandler>(); 
builder.Services.AddHttpClient("WorkTracker-API", client => client.BaseAddress = new Uri(builder.Configuration.GetValue<string>("API:Url")))
    .AddHttpMessageHandler<APIAuthorizationMessageHandler>();

// Supply HttpClient instances that include access tokens when making requests to the server project
builder.Services.AddScoped(sp => sp.GetRequiredService<IHttpClientFactory>().CreateClient("WorkTracker-API"));

builder.Services.AddMsalAuthentication(options =>
{
    builder.Configuration.Bind("AzureAd", options.ProviderOptions.Authentication);
    options.ProviderOptions.DefaultAccessTokenScopes.Add(builder.Configuration.GetValue<string>("AzureAd:Scope"));
});

builder.Services.AddTransient<IWorkTrackerApiService, WorkTrackerApiService>();
builder.Services.AddTransient<DialogService>();

await builder.Build().RunAsync();
