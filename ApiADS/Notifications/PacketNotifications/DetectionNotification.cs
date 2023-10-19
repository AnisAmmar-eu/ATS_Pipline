using Core.Entities.Packets.Models.Structs;
using Core.Shared.Dictionaries;
using TwinCAT.Ads;

namespace ApiADS.Notifications.PacketNotifications;

public class DetectionNotification : PacketNotification<DetectionStruct>
{
	public DetectionNotification()
	{
		_toDequeue = false;
	}

	// TODO Do NOT dequeue it.
	public static async Task<DetectionNotification> Create(dynamic ads)
	{
		return (await CreateSub<DetectionNotification>(ads, ADSUtils.DetectionRemove,
			ADSUtils.DetectionNewMsg, ADSUtils.DetectionToRead) as DetectionNotification)!;
	}
}