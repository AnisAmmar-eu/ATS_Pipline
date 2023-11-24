using Core.Entities.Packets.Models.DB;
using Core.Entities.Packets.Models.DTO;
using Core.Shared.Data;
using Core.Shared.Repositories.Kernel;

namespace Core.Entities.Packets.Repositories;

public class PacketRepository : BaseEntityRepository<AnodeCTX, Packet, DTOPacket>, IPacketRepository
{
	public PacketRepository(AnodeCTX context) : base(context)
	{
	}
}