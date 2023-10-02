using ApiADS.Notifications;
using Microsoft.AspNetCore.Mvc;
using TwinCAT.Ads;

namespace ApiADS.Controllers;

[ApiController]
[Route("ads")]
public class ADSController : ControllerBase
{
	private IServiceProvider _serviceProvider;

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
				if (tcClient.IsConnected)
				{
					break;
				}

				Console.WriteLine("Unable to connect to the automaton. Retrying in 1 second");
				Thread.Sleep(1000);
			}

			// Create dynamic Object to use in function notification
			dynamic ads = new System.Dynamic.ExpandoObject();
			ads.tcClient = tcClient;
			ads.appServices = _serviceProvider;
			ads.cancel = cancel;
			AlarmNotification alarmNotification = AlarmNotification.Create(ads);
			try
			{
				while ((await tcClient.ReadAnyAsync<uint>(ads.alarmNew, cancel)).ErrorCode ==
				       AdsErrorCode.NoError)
				{
					// To avoid spamming the TwinCat
					Thread.Sleep(1000);
				}
			}
			catch (Exception ex)
			{
				Console.WriteLine(ex.ToString());
				return Ok();
			}
		}
	}
}