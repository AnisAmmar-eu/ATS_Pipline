using Core.Entities.Packets.Models.Structs;
using Core.Shared.Dictionaries;

namespace Core.Shared.Services.Notifications.PacketNotifications;

public class DetectionNotification : PacketNotification<DetectionStruct>
{
	public DetectionNotification()
	{
		ToDequeue = false;
	}

	public static async Task<DetectionNotification> Create(dynamic ads)
	{
		return (await CreateSub<DetectionNotification>(ads, ADSUtils.DetectionRemove,
			ADSUtils.DetectionNewMsg, ADSUtils.DetectionToRead) as DetectionNotification)!;
	}
}