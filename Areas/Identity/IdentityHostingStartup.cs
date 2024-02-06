using Microsoft.AspNetCore.Hosting;

[assembly: HostingStartup(typeof(Gamification.UI.Areas.Identity.IdentityHostingStartup))]
namespace Gamification.UI.Areas.Identity
{
	public class IdentityHostingStartup : IHostingStartup
	{
		public void Configure(IWebHostBuilder builder)
		{
			builder.ConfigureServices((context, services) =>
			{
			});
		}
	}
}