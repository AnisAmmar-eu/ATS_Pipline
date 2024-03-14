using Core.Entities.Alarms.AlarmsCycle.Models.DTO;
using Core.Entities.Packets.Models.DB;
using Core.Entities.Packets.Models.DB.Shootings;
using Core.Entities.Packets.Models.DTO;
using Core.Entities.Packets.Models.DTO.Shootings;
using Core.Entities.StationCycles.Models.DB;
using Core.Shared.Services.Kernel.Interfaces;
using Microsoft.AspNetCore.Http;

namespace Core.Entities.Packets.Services;

public interface IPacketService : IBaseEntityService<Packet, DTOPacket>
{
	public Task<DTOShooting> GetMostRecentShooting();

	/// <summary>
	/// This photo returns the FileInfo of the image gotten through the shooting packet. It works only on the station.
	/// </summary>
	/// <param name="ShootingID"></param>
	/// <param name="cameraID"></param>
	/// <returns></returns>
	public Task<FileInfo> GetImageFromCycleRIDAndCamera(int ShootingID, int cameraID);

	/// <summary>
	/// Executes side effects on a packet when called. Allows a packet to update itself on other factors determined by
	/// the implementation of a InheritedBuild function.
	/// </summary>
	/// <param name="packet"></param>
	/// <returns></returns>
	public Task<DTOPacket> BuildPacket(Packet packet);

	/// <summary>
	/// Sends all packets given in argument to the given serverAddress, returns all packets which were successfully sent
	/// with their status as "Sent".
	/// </summary>
	/// <returns></returns>
	public Task SendCompletedPackets();

	/// <summary>
	/// Receives a <see cref="Packet"/> from a station and saves it in the database. If needed, creates its corresponding <see cref="StationCycle"/>
	/// </summary>
	/// <param name="dtoPacket"></param>
	/// <param name="stationName"></param>
	/// <returns></returns>
	public Task ReceivePacket(DTOPacket dtoPacket, string stationName);

	/// <summary>
	/// Receives the images of a <see cref="Shooting"/> packet from a station and saves them at the right location.
	/// However, receiving the <see cref="Shooting"/> packet is managed by <see cref="ReceivePacket"/>
	/// </summary>
	/// <param name="formFiles"></param>
	/// <param name="isImage"></param>
	/// <returns></returns>
	public Task ReceiveStationImage(IFormFileCollection formFiles, bool isImage);

	/// <summary>
	/// Receives the alarm cycles.
	/// </summary>
	/// <param name="dtoAlarmCycles">The list of DTO alarm cycles.</param>
	/// /// <param name="stationName"></param>
	/// /// <param name="cycleRID"></param>
	/// <returns>A task representing the asynchronous operation.</returns>
	public Task ReceivePacketAlarm(List<DTOAlarmCycle> dtoAlarmCycles, string stationName, string cycleRID);
}