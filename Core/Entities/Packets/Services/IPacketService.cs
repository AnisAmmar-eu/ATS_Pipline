using Core.Entities.Packets.Models.DB;
using Core.Entities.Packets.Models.DTO;
using Core.Shared.Services.Kernel.Interfaces;

namespace Core.Entities.Packets.Services;

public interface IPacketService : IServiceBaseEntity<Packet, DTOPacket>
{
	public Task<DTOPacket> BuildPacket(Packet packet);
	public Task<List<Packet>> ReceivePackets(IEnumerable<DTOPacket> packet);
	public Task<int?> AddPacketFromStationCycle(Packet? packet);
	public void MarkPacketAsSentFromStationCycle(Packet? packet);
}