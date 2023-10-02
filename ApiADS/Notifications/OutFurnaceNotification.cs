using Core.Entities.Packets.Models.DB;
using Core.Entities.Packets.Models.DTO;
using Core.Entities.Packets.Models.Structs;
using Core.Entities.Packets.Services;
using TwinCAT.Ads;

namespace ApiADS.Notifications;

public class OutFurnaceNotification : BaseNotification<IPacketService, Packet, DTOPacket, OutFurnaceStruct>
{
	public OutFurnaceNotification(ResultHandle resultHandle, uint acquitMsg, uint newMsg, uint oldEntry)
		: base(resultHandle, acquitMsg, newMsg, oldEntry)
	{
	}
	
	public new static async Task<OutFurnaceNotification> Create(dynamic ads)
	{
		return await CreateSub(ads, Utils.OutFurnaceAcquitMsg, Utils.OutFurnaceNewMsg, Utils.OutFurnaceToRead);
	}
}