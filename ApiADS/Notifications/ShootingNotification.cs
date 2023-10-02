using Core.Entities.Packets.Models.DB;
using Core.Entities.Packets.Models.DTO;
using Core.Entities.Packets.Models.Structs;
using Core.Entities.Packets.Services;
using TwinCAT.Ads;

namespace ApiADS.Notifications;

public class ShootingNotification : BaseNotification<IPacketService, Packet, DTOPacket, ShootingStruct>
{
	public ShootingNotification(ResultHandle resultHandle, uint acquitMsg, uint newMsg, uint oldEntry)
		: base(resultHandle, acquitMsg, newMsg, oldEntry)
	{
	}

	public static async Task<ShootingNotification> Create(dynamic ads)
	{
		return await CreateSub(ads, Utils.ShootingAcquitMsg, Utils.ShootingNewMsg, Utils.ShootingToRead);
	}
}