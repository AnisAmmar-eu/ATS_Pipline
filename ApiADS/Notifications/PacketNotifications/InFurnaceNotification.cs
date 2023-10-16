using Core.Entities.Packets.Models.Structs;
using TwinCAT.Ads;

namespace ApiADS.Notifications.PacketNotifications;

public class InFurnaceNotification : PacketNotification<InFurnaceStruct>
{
	public InFurnaceNotification(ResultHandle resultHandle, uint acquitMsg, uint newMsg, uint oldEntry)
		: base(resultHandle, acquitMsg, newMsg, oldEntry)
	{
	}

	public static async Task<InFurnaceNotification> Create(dynamic ads)
	{
		return await CreateSub(ads, Utils.InFurnaceAcquitMsg, Utils.InFurnaceNewMsg, Utils.InFurnaceToRead);
	}
}