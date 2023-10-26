using Core.Entities.Packets.Models.DB;
using Core.Entities.Packets.Models.DTO;
using Core.Entities.Packets.Services;
using Core.Shared.Models.DB.Kernel.Interfaces;
using Core.Shared.SignalR.StationCycleHub;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.DependencyInjection;

namespace Core.Shared.Services.Notifications.PacketNotifications;

public class PacketNotification<TStruct> : BaseNotification<IPacketService, Packet, DTOPacket, TStruct>
	where TStruct : struct, IBaseADS<Packet, TStruct>
{
	protected override async Task AddElement(IServiceProvider services, Packet entity)
	{
		IPacketService packetService = services.GetRequiredService<IPacketService>();
		IHubContext<StationCycleHub, IStationCycleHub> hubContext =
			services.GetRequiredService<IHubContext<StationCycleHub, IStationCycleHub>>();
		await packetService.BuildPacket(entity);
		await hubContext.Clients.All.RefreshStationCycle();
	}
}