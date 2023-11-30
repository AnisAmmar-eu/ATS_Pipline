using Core.Entities.Packets.Models.Structs;
using Core.Shared.Dictionaries;

namespace Core.Shared.Services.Notifications.PacketNotifications;

public class InFurnaceNotification : PacketNotification<InFurnaceStruct>
{
	public static async Task<InFurnaceNotification> Create(dynamic ads)
	{
		return (await CreateSub<InFurnaceNotification>(
			ads,
			ADSUtils.InFurnaceRemove,
			ADSUtils.InFurnaceNewMsg,
			ADSUtils.InFurnaceToRead) as InFurnaceNotification)!;
	}
}