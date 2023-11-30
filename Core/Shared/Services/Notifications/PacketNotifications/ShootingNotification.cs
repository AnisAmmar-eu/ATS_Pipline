using Core.Entities.Packets.Models.Structs;
using Core.Shared.Dictionaries;

namespace Core.Shared.Services.Notifications.PacketNotifications;

public class ShootingNotification : PacketNotification<ShootingStruct>
{
	public static async Task<ShootingNotification?> Create(dynamic ads)
	{
		return await CreateSub<ShootingNotification>(
			ads,
			ADSUtils.ShootingAcquitMsg,
			ADSUtils.ShootingNewMsg,
			ADSUtils.ShootingToRead) as ShootingNotification;
	}
}