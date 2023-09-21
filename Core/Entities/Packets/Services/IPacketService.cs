using Core.Entities.Packets.Models.DTO;

namespace Core.Entities.Packets.Services;

public interface IPacketService
{
	public Task<DTOPacket> BuildPacket(DTOPacket dtoPacket);
}