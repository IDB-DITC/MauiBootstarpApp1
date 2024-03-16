using MauiBoostarpApp1.Security;
using MauiBoostarpApp1.Services;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.Extensions.Logging;
using System.Net.Http;
using Syncfusion.Blazor;


namespace MauiBoostarpApp1
{
	public static class MauiProgram
	{
		public static MauiApp CreateMauiApp()
		{
			var builder = MauiApp.CreateBuilder();


			builder
				.UseMauiApp<App>()
				
				.ConfigureFonts(fonts =>
				{
					fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
				});




			builder.Services.AddSyncfusionBlazor();




			builder.Services.AddSingleton<IPlatformHttpMessageHandler>(sp =>
			{

#if DEBUG

                Console.WriteLine("app ruunig in debug mode");

#endif

#if ANDROID

                return new AndroidHttpMessageHandler();

#else 


                return new WebHttpMessageHandler();

				#endif

			});
			//builder.Services.AddHttpClient<AuthService>(http =>
			//{
   //             var baseAddress = DeviceInfo.Platform == DevicePlatform.Android ? "https://10.0.2.2:7115" : "https://localhost:7115";
   //             http.BaseAddress = new Uri(baseAddress);
   //         }).ConfigurePrimaryHttpMessageHandler(sp => sp.GetRequiredService<IPlatformHttpMessageHandler>().GetHttpMessageHandler());

            builder.Services.AddHttpClient("custom-httpclient", httpClient =>
			{
				var baseAddress = DeviceInfo.Platform == DevicePlatform.Android ? "https://10.0.2.2:7115" : "https://localhost:7115";
				httpClient.BaseAddress = new Uri(baseAddress);
			})
				.ConfigurePrimaryHttpMessageHandler(sp => sp.GetRequiredService<IPlatformHttpMessageHandler>().GetHttpMessageHandler());

            builder.Services.AddSingleton<AuthService>();

            builder.Services.AddSingleton<ProductService>();



            builder.Services.AddScoped<AuthenticationStateProvider, JwtAuthStateProvider>();

            builder.Services.AddCascadingAuthenticationState();

            builder.Services.AddAuthorizationCore();





			//builder.Services.AddSingleton<ClientService>();






			//builder.Services.AddSingleton<IHttpsClientHandlerService, HttpsClientHandlerService>();


			//builder.Services.AddSingleton<HttpClient>();




			builder.Services.AddMauiBlazorWebView();

#if DEBUG
			builder.Services.AddBlazorWebViewDeveloperTools();
			builder.Logging.AddDebug();
#endif

			return builder.Build();
		}
	}
}
