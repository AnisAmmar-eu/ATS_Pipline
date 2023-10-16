using Core.Entities.Packets.Models.DB;
using Core.Entities.Packets.Models.DTO;
using Core.Shared.Services.Kernel.Interfaces;

namespace Core.Entities.Packets.Services;

public interface IPacketService : IServiceBaseEntity<Packet, DTOPacket>
{
	public Task<DTOPacket> BuildPacket(Packet packet);
	public Task<HttpResponseMessage> SendPacketsToServer();
	public Task ReceivePacket(IEnumerable<DTOPacket> packet);
}