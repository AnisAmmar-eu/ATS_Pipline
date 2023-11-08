using Core.Entities.IOT.Data;
using Core.Entities.KPI.Data;
using Core.Entities.User.Data;
using Core.Entities.User.Models.DB.Users;
using Microsoft.AspNetCore.Identity;

namespace Core.Shared.Data;

public class DBInitializer
{
	public static async Task InitializeStation(AnodeCTX anodeCTX, UserManager<ApplicationUser> userManager)
	{
		IOTInitializer.Initialize(anodeCTX);
		KPIInitializer.Initialize(anodeCTX);
		await UserInitializer.Initialize(anodeCTX, userManager);
	}

	public static Task InitializeServer(AnodeCTX anodeCTX)
	{
		KPIInitializer.Initialize(anodeCTX);
		return Task.CompletedTask;
	}
}