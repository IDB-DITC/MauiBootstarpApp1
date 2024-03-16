

namespace MauiBoostarpApp1
{
	public interface IPlatformHttpMessageHandler
	{
		HttpMessageHandler GetHttpMessageHandler();
	}



	public class WebHttpMessageHandler : IPlatformHttpMessageHandler
	{
		public HttpMessageHandler GetHttpMessageHandler() => new HttpClientHandler();
	}
	
}
