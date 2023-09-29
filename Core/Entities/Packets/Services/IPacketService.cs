using Core.Entities.Packets.Models.DB;
using Core.Entities.Packets.Models.DTO;

namespace Core.Entities.Packets.Services;

public interface IPacketService
{
	public Task<DTOPacket> AddPacket(Packet packet);
	public Task<DTOPacket> BuildPacket(DTOPacket dtoPacket);
	public Task<HttpResponseMessage> SendPacketsToServer();
	public Task ReceivePacket(IEnumerable<DTOPacket> packet);
}