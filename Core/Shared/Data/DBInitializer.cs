using Core.Entities.IOT.Data;
using Core.Entities.User.Data;
using Core.Entities.User.Models.DB.Users;
using Microsoft.AspNetCore.Identity;

namespace Core.Shared.Data;

public class DBInitializer
{
	public static async Task Initialize(AnodeCTX anodeCTX, UserManager<ApplicationUser> userManager)
	{
		IOTInitializer.Initialize(anodeCTX);
		await UserInitializer.Initialize(anodeCTX, userManager);
	}
}