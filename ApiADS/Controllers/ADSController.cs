using System.Dynamic;
using ApiADS.Notifications.PacketNotifications;
using Core.Shared.Dictionaries;
using Microsoft.AspNetCore.Mvc;
using TwinCAT.Ads;

namespace ApiADS.Controllers;

[ApiController]
[Route("ads")]
public class ADSController : ControllerBase
{
	private readonly IServiceProvider _serviceProvider;

	public ADSController(IServiceProvider serviceProvider)
	{
		_serviceProvider = serviceProvider;
	}

	[HttpPost("s1s2")]
	public async Task<IActionResult> S1S2ADS()
	{
		while (true)
		{
			CancellationToken cancel = CancellationToken.None;
			AdsClient tcClient = new();
			while (true)
			{
				// Connection
				tcClient.Connect(851);
				if (tcClient.IsConnected) break;

				Console.WriteLine("Unable to connect to the automaton. Retrying in 1 second");
				Thread.Sleep(1000);
			}

			// Create dynamic Object to use in function notification
			dynamic ads = new ExpandoObject();
			ads.tcClient = tcClient;
			ads.appServices = _serviceProvider;
			ads.cancel = cancel;
			await AnnouncementNotification.Create(ads);
			await DetectionNotification.Create(ads);
			// AlarmNotification.Create(ads);
			try
			{
				uint handle = tcClient.CreateVariableHandle(ADSUtils.AnnouncementNewMsg);
				while ((await tcClient.ReadAnyAsync<bool>(handle, cancel)).ErrorCode ==
				       AdsErrorCode.NoError)
					// To avoid spamming the TwinCat
					Thread.Sleep(1000);
			}
			catch (Exception ex)
			{
				Console.WriteLine(ex.ToString());
				return Ok();
			}
		}
	}
}