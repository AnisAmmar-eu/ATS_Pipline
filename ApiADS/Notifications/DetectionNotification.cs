using Core.Entities.Packets.Models.DB;
using Core.Entities.Packets.Models.DTO;
using Core.Entities.Packets.Models.Structs;
using Core.Entities.Packets.Services;
using TwinCAT.Ads;

namespace ApiADS.Notifications;

public class DetectionNotification : BaseNotification<IPacketService, Packet, DTOPacket, DetectionStruct>
{
	public DetectionNotification(ResultHandle resultHandle, uint acquitMsg, uint newMsg, uint oldEntry)
		: base(resultHandle, acquitMsg, newMsg, oldEntry)
	{
	}

	public new static async Task<DetectionNotification> Create(dynamic ads)
	{
		return await CreateSub(ads, Utils.DetectionAcquitMsg, Utils.DetectionNewMsg, Utils.DetectionToRead);
	}
}