using Core.Entities.Packets.Models.Structs;
using Core.Shared.Dictionaries;

namespace ApiADS.Notifications.PacketNotifications;

public class OutFurnaceNotification : PacketNotification<OutFurnaceStruct>
{
	public static async Task<OutFurnaceNotification> Create(dynamic ads)
	{
		return (await CreateSub<OutFurnaceNotification>(ads, ADSUtils.OutFurnaceRemove,
			ADSUtils.OutFurnaceNewMsg, ADSUtils.OutFurnaceToRead) as OutFurnaceNotification)!;
	}
}