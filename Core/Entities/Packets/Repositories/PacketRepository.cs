using Core.Entities.Packets.Models.DB;
using Core.Entities.Packets.Models.DTO;
using Core.Shared.Data;
using Core.Shared.Repositories.Kernel;

namespace Core.Entities.Packets.Repositories;

public class PacketRepository : RepositoryBaseEntity<AlarmCTX, Packet, DTOPacket>, IPacketRepository
{
	public PacketRepository(AlarmCTX context) : base(context)
	{
	}
}