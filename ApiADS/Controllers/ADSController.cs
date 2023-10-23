using System.Dynamic;
using Core.Shared.Dictionaries;
using Core.Shared.Services.Notifications.PacketNotifications;
using Microsoft.AspNetCore.Mvc;
using TwinCAT.Ads;

namespace ApiADS.Controllers;

[ApiController]
[Route("ads")]
public class ADSController : ControllerBase
{
	public ADSController()
	{
	}
}