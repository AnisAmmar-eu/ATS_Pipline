using Core.Entities.Alarms.Data;
using Core.Entities.IOT.Data;
using Core.Entities.KPI.Data;
using Core.Entities.User.Data;
using Core.Entities.User.Models.DB.Users;
using Core.Entities.Vision.Data;
using Microsoft.AspNetCore.Identity;

namespace Core.Shared.Data;

/// <summary>
/// Functions to be called upon startup of one Api per station/server to initialise the database.
/// </summary>
public static class DBInitializer
{
	public static Task InitializeStation(AnodeCTX anodeCTX, UserManager<ApplicationUser> userManager)
	{
		IOTInitializer.InitializeStation(anodeCTX);
		AlarmInitializer.InitializeStation(anodeCTX);
		return UserInitializer.Initialize(anodeCTX, userManager);
	}

	public static Task InitializeServer(AnodeCTX anodeCTX, UserManager<ApplicationUser> userManager)
	{
		IOTInitializer.InitializeServer(anodeCTX);
		// TODO Don't. RIDs are the same station by station but need to be different on the server
		AlarmInitializer.InitializeStation(anodeCTX);
		KPIInitializer.Initialize(anodeCTX);
		return UserInitializer.Initialize(anodeCTX, userManager);
	}
}