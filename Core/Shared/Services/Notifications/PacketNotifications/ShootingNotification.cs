using Core.Entities.Packets.Models.Structs;
using Core.Shared.Dictionaries;
using Microsoft.Extensions.Logging;

namespace Core.Shared.Services.Notifications.PacketNotifications;

public class ShootingNotification : PacketNotification<ShootingStruct>
{
	public static async Task<ShootingNotification?> Create(dynamic ads, ILogger logger)
	{
		return await CreateSub<ShootingNotification>(
			ads,
			ADSUtils.ShootingRemove,
			ADSUtils.ShootingNewMsg,
			ADSUtils.ShootingToRead,
			logger) as ShootingNotification;
	}
}