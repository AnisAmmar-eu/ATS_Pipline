using Core.Entities.Packets.Models.DB;
using Core.Entities.Packets.Models.DTO;
using Core.Entities.Packets.Services;
using Core.Shared.Models.DB.Kernel.Interfaces;
using TwinCAT.Ads;

namespace ApiADS.Notifications.PacketNotifications;

public class PacketNotification<TStruct> : BaseNotification<IPacketService, Packet, DTOPacket, TStruct>
	where TStruct : struct, IBaseADS<Packet, TStruct>
{
	public PacketNotification(ResultHandle resultHandle, uint remove, uint newMsg, uint oldEntry)
		: base(resultHandle, remove, newMsg, oldEntry)
	{
	}

	public PacketNotification()
	{
	}

	protected override async Task AddElement(IServiceProvider services, Packet entity)
	{
		IPacketService packetService = services.GetRequiredService<IPacketService>();
		await packetService.BuildPacket(entity);
	}
}