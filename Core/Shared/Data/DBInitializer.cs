using Core.Entities.IOT.Data;
using Core.Entities.KPI.Data;
using Core.Entities.User.Data;
using Core.Entities.User.Models.DB.Users;
using Core.Entities.Vision.Data;
using Microsoft.AspNetCore.Identity;

namespace Core.Shared.Data;

public static class DBInitializer
{
	public static Task InitializeStation(AnodeCTX anodeCTX)
	{
		IOTInitializer.InitializeStation(anodeCTX);
		return Task.CompletedTask;
	}

	public static async Task InitializeServer(AnodeCTX anodeCTX, UserManager<ApplicationUser> userManager)
	{
		IOTInitializer.InitializeServer(anodeCTX);
		KPIInitializer.Initialize(anodeCTX);
		VisionInitializer.Initialize(anodeCTX);
		await UserInitializer.Initialize(anodeCTX, userManager);
	}
}