using Core.Entities.Packets.Models.Structs;
using TwinCAT.Ads;

namespace ApiADS.Notifications.PacketNotifications;

public class OutFurnaceNotification : PacketNotification<OutFurnaceStruct>
{
	public OutFurnaceNotification(ResultHandle resultHandle, uint acquitMsg, uint newMsg, uint oldEntry)
		: base(resultHandle, acquitMsg, newMsg, oldEntry)
	{
	}

	public static async Task<OutFurnaceNotification> Create(dynamic ads)
	{
		return (await CreateSub<PacketNotification<OutFurnaceStruct>>(ads, Utils.OutFurnaceAcquitMsg,
			Utils.OutFurnaceNewMsg, Utils.OutFurnaceToRead) as OutFurnaceNotification)!;
	}
}