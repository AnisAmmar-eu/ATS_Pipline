using Core.Entities.Packets.Models.DB;
using Core.Entities.Packets.Models.DTO;
using Core.Shared.Repositories.Kernel.Interfaces;

namespace Core.Entities.Packets.Repositories;

public interface IPacketRepository : IRepositoryBaseEntity<Packet, DTOPacket>
{
	
}