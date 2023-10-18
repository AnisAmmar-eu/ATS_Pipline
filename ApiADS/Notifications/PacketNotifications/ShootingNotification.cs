using Core.Entities.Packets.Models.Structs;
using TwinCAT.Ads;

namespace ApiADS.Notifications.PacketNotifications;

public class ShootingNotification : PacketNotification<ShootingStruct>
{
	public ShootingNotification(ResultHandle resultHandle, uint acquitMsg, uint newMsg, uint oldEntry)
		: base(resultHandle, acquitMsg, newMsg, oldEntry)
	{
	}

	public static async Task<ShootingNotification?> Create(dynamic ads)
	{
		return await CreateSub<PacketNotification<ShootingStruct>>(ads, Utils.ShootingAcquitMsg, Utils.ShootingNewMsg,
			Utils.ShootingToRead) as ShootingNotification;
	}
}