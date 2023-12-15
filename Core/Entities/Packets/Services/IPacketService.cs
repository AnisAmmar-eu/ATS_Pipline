using Core.Entities.Packets.Models.DB;
using Core.Entities.Packets.Models.DB.Shootings;
using Core.Entities.Packets.Models.DTO;
using Core.Shared.Services.Kernel.Interfaces;
using Microsoft.AspNetCore.Http;

namespace Core.Entities.Packets.Services;

public interface IPacketService : IBaseEntityService<Packet, DTOPacket>
{
	public Task<Shooting> GetMostRecentShooting();

	/// <summary>
	/// This photo returns the FileInfo of the image gotten through the shooting packet. It works only on the station.
	/// </summary>
	/// <param name="shootingID"></param>
	/// <param name="cameraID"></param>
	/// <returns></returns>
	public Task<FileInfo> GetImageFromIDAndCamera(int shootingID, int cameraID);
	public Task<DTOPacket> BuildPacket(Packet packet);

	/// <summary>
	/// Sends all packets given in argument to the given serverAddress, returns all packets which were successfully sent
	/// with their status as "Sent".
	/// </summary>
	/// <param name="imagesPath"></param>
	/// <returns></returns>
	public Task SendCompletedPackets(string imagesPath);

	public Task ReceivePacket(DTOPacket dtoPacket, string stationName);
	public Task ReceiveStationImage(IFormFileCollection formFiles);
}