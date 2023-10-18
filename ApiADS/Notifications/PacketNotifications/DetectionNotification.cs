using Core.Entities.Packets.Models.Structs;
using TwinCAT.Ads;

namespace ApiADS.Notifications.PacketNotifications;

public class DetectionNotification : PacketNotification<DetectionStruct>
{
	public DetectionNotification(ResultHandle resultHandle, uint acquitMsg, uint newMsg, uint oldEntry)
		: base(resultHandle, acquitMsg, newMsg, oldEntry)
	{
	}

	public static async Task<DetectionNotification> Create(dynamic ads)
	{
		return (await CreateSub<PacketNotification<DetectionStruct>>(ads, Utils.DetectionRemove,
			Utils.DetectionNewMsg, Utils.DetectionToRead) as DetectionNotification)!;
	}
}