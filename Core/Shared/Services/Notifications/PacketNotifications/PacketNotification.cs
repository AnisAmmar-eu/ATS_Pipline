using Core.Entities.Packets.Models.DB;
using Core.Entities.Packets.Models.DTO;
using Core.Entities.Packets.Services;
using Core.Shared.Models.DB.Kernel.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace Core.Shared.Services.Notifications.PacketNotifications;

public class PacketNotification<TStruct> : BaseNotification<IPacketService, Packet, DTOPacket, TStruct>
	where TStruct : struct, IBaseADS<Packet, TStruct>
{
	protected override async Task AddElement(IServiceProvider services, Packet entity)
	{
		IPacketService packetService = services.GetRequiredService<IPacketService>();
		await packetService.BuildPacket(entity);
	}
}