using Core.Entities.Packets.Models.DB;
using Core.Entities.Packets.Models.DTO;
using Core.Entities.Packets.Models.Structs;
using Core.Entities.Packets.Services;
using TwinCAT.Ads;

namespace ApiADS.Notifications;

public class InFurnaceNotification : BaseNotification<IPacketService, Packet, DTOPacket, InFurnaceStruct>
{
	public InFurnaceNotification(ResultHandle resultHandle, uint acquitMsg, uint newMsg, uint oldEntry)
		: base(resultHandle, acquitMsg, newMsg, oldEntry)
	{
	}

	public new static async Task<InFurnaceNotification> Create(dynamic ads)
	{
		return await CreateSub(ads, Utils.InFurnaceAcquitMsg, Utils.InFurnaceNewMsg, Utils.InFurnaceToRead);
	}
}