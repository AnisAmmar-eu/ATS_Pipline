using Core.Entities.Alarms.AlarmsPLC.Models.DTO;
using Core.Entities.Packets.Models.DB;
using Core.Entities.Packets.Models.DTO;
using Core.Shared.Services.Kernel.Interfaces;

namespace Core.Entities.Packets.Services;

public interface IPacketService : IServiceBaseEntity<Packet, DTOPacket>
{
	public Task<DTOPacket> BuildPacket(DTOPacket dtoPacket);
	public Task<HttpResponseMessage> SendPacketsToServer();
	public Task ReceivePacket(IEnumerable<DTOPacket> packet);
}