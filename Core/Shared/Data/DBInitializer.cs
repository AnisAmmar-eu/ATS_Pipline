using Core.Entities.IOT.Data;
using Core.Entities.KPI.Data;
using Core.Entities.User.Data;
using Core.Entities.User.Models.DB.Users;
using Core.Entities.Vision.Data;
using Microsoft.AspNetCore.Identity;

namespace Core.Shared.Data;

public static class DBInitializer
{
	public static Task InitializeStation(AnodeCTX anodeCTX, UserManager<ApplicationUser> userManager)
	{
		IOTInitializer.InitializeStation(anodeCTX);
		return UserInitializer.Initialize(anodeCTX, userManager);
	}

	public static Task InitializeServer(AnodeCTX anodeCTX, UserManager<ApplicationUser> userManager)
	{
		IOTInitializer.InitializeServer(anodeCTX);
		KPIInitializer.Initialize(anodeCTX);
		VisionInitializer.Initialize(anodeCTX);
		return UserInitializer.Initialize(anodeCTX, userManager);
	}
}