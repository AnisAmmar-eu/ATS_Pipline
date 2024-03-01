using System.Net;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using Core.Entities.Alarms.AlarmsCycle.Models.DB;
using Core.Entities.Alarms.AlarmsCycle.Models.DTO;
using Core.Entities.IOT.Dictionaries;
using Core.Entities.Packets.Dictionaries;
using Core.Entities.Packets.Models.DB;
using Core.Entities.Packets.Models.DB.AlarmLists;
using Core.Entities.Packets.Models.DB.Shootings;
using Core.Entities.Packets.Models.DTO;
using Core.Entities.Packets.Models.DTO.AlarmLists;
using Core.Entities.Packets.Models.DTO.Shootings;
using Core.Entities.Packets.Repositories;
using Core.Entities.StationCycles.Models.DB;
using Core.Shared.Configuration;
using Core.Shared.Dictionaries;
using Core.Shared.Exceptions;
using Core.Shared.Models.ApiResponses;
using Core.Shared.Services.Kernel;
using Core.Shared.UnitOfWork.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Stemmer.Cvb;

namespace Core.Entities.Packets.Services;

public class PacketService : BaseEntityService<IPacketRepository, Packet, DTOPacket>, IPacketService
{
	private readonly ILogger<PacketService> _logger;
	private readonly IConfiguration _configuration;

	public PacketService(IAnodeUOW anodeUOW, ILogger<PacketService> logger, IConfiguration configuration) : base(anodeUOW)
	{
		_logger = logger;
		_configuration = configuration;
	}

	public async Task<DTOShooting> GetMostRecentShooting()
	{
		// Here, we order by descending ID and not by TS because in a **single** station, the most recent packets always
		// have the highest IDs. In the server, we would have to order by TS.
		return (await AnodeUOW.Packet
			.GetBy(
				[packet => packet is Shooting],
				orderBy: query => query.OrderByDescending(packet => packet.ID)) as Shooting
			?? throw new EntityNotFoundException())
			.ToDTO();
	}

	public async Task<FileInfo> GetImageFromCycleRIDAndCamera(string stationCycleRID, int cameraID)
	{
		Shooting shooting = await AnodeUOW.Packet.GetBy([packet => packet.StationCycleRID == stationCycleRID]) as Shooting
			?? throw new EntityNotFoundException("Found a packet but it is not a shooting one");

		string thumbnailsPath = _configuration.GetValueWithThrow<string>(ConfigDictionary.ThumbnailsPath);
		string extension = _configuration.GetValueWithThrow<string>(ConfigDictionary.CameraExtension);

		return Shooting.GetImagePathFromRoot(
			shooting.StationCycleRID,
			Station.ID,
			thumbnailsPath,
			shooting.AnodeType,
			cameraID,
			extension);
	}

	public async Task<DTOPacket> BuildPacket(Packet packet)
	{
		await AnodeUOW.StartTransaction();

		await packet.Create(AnodeUOW);

		await packet.Build(AnodeUOW);

		await AnodeUOW.CommitTransaction();
		return packet.ToDTO();
	}

	public async Task SendCompletedPackets(string imagesPath)
	{
		string extension = _configuration.GetValueWithThrow<string>(ConfigDictionary.CameraExtension);
		IEnumerable<Packet> packets
			= await AnodeUOW.Packet.GetAll([packet => packet.Status == PacketStatus.Completed]);
		if (!packets.Any())
			return;

		await AnodeUOW.StartTransaction();
		foreach (Packet packet in packets)
		{
			try
			{
				using HttpClient http = new();
				_logger.LogInformation("Sending packet: {packetID}", packet.ID);
				_logger.LogInformation("Sending is shooting: {isShooting}", packet is Shooting);
				_logger.LogInformation("Sending is alarm list: {isAlarmList}", packet is AlarmList);

				if (packet is Shooting shooting)
				{
					await shooting.SendImages(imagesPath, extension, _logger);
					_logger.LogInformation("Sending images: {packet}", shooting.ToDTO());
					_logger.LogInformation("Sending images: {packet}", shooting.ToDTO().Type);
					_logger.LogInformation("Sending images: {packet}", shooting.ToDTO().GetType());
					HttpResponseMessage response = await http.PostAsJsonAsync(
						$"{ITApisDict.ServerReceiveAddress}/apiServerReceive/{Station.Name}/packets",
						shooting.ToDTO(),
						ApiResponse.JsonOptions);
					if (response.StatusCode != HttpStatusCode.OK)
					{
						throw new("Send packet to server failed with status code:"
							+ $" {response.StatusCode.ToString()}\nReason: {response.ReasonPhrase}");
					}
				}

				if (packet is AlarmList alarmLists)
				{
					// Get all alarmCycle
					List<DTOAlarmCycle> dtoAlarmCycles = (await AnodeUOW.AlarmCycle
						.GetAll([alarmCycle => alarmCycle.AlarmListPacketID == alarmLists.ID], withTracking: false))
						.ConvertAll(alarmCycle => alarmCycle.ToDTO());

					StringContent content
					= new(JsonSerializer.Serialize(dtoAlarmCycles, ApiResponse.JsonOptions), Encoding.UTF8, "application/json");
					HttpResponseMessage response = await http.PostAsync(
						$"{ITApisDict.ServerReceiveAddress}/apiServerReceive/{Station.Name}/AlarmPacket/{alarmLists.StationCycleRID}",
						content);
					if (response.StatusCode != HttpStatusCode.OK)
					{
						throw new("Send packet to server failed with status code:"
							+ $" {response.StatusCode.ToString()}\nReason: {response.ReasonPhrase}");
					}
				}

				packet.Status = PacketStatus.Sent;
				AnodeUOW.Packet.Update(packet);
				_logger.LogInformation("Packet sent: {packetID}", packet.ID);
			}
			catch (Exception e)
			{
				_logger.LogError("Error when sending packet: {e}", e);
			}
		}

		AnodeUOW.Commit();
		await AnodeUOW.CommitTransaction();
	}

	public async Task ReceivePacket(DTOPacket dtoPacket, string stationName)
	{
		_logger.LogInformation("MARCO1 DToPacket: {dtoPacket}", dtoPacket);
		_logger.LogInformation("MARCO1 DToPacket: {dtoPacketType}", dtoPacket.Type);
		_logger.LogInformation("MARCO1 DToPacket: {dtoPacketType}", dtoPacket.GetType());
		Packet packet = dtoPacket.ToModel();
		packet.ID = 0;
		await AnodeUOW.StartTransaction();
		_logger.LogInformation("MARCO2");
		await AnodeUOW.Packet.Add(packet);
		AnodeUOW.Commit();
		_logger.LogInformation("MARCO3");
		try
		{
			StationCycle stationCycle
				= await AnodeUOW.StationCycle.GetBy([cycle => cycle.RID == packet.StationCycleRID], withTracking: false);
			stationCycle.AssignPacket(packet);
			AnodeUOW.StationCycle.Update(stationCycle);
			AnodeUOW.Commit();
		}
		catch (EntityNotFoundException)
		{
			if (packet is Shooting shooting)
			{
				StationCycle stationCycle = StationCycle.Create(stationName);
				stationCycle.StationID = Station.StationNameToID(stationName);
				stationCycle.AnodeType = shooting.AnodeType;
				stationCycle.RID = shooting.StationCycleRID;
				stationCycle.ShootingPacket = shooting;
				_logger.LogInformation("POLO1");
				// Every "orphan" packet is aggregated to this stationCycle.
				List<Packet> packets
					= await AnodeUOW.Packet
						.GetAll(
							[packet1 => packet1.StationCycleRID == stationCycle.RID, packet2 => packet2.ID != shooting.ID],
							withTracking: false);
				_logger.LogInformation("POLO2");
				packets.ForEach(stationCycle.AssignPacket);
				await AnodeUOW.StationCycle.Add(stationCycle);
				_logger.LogInformation("POLO3");
				AnodeUOW.Commit();
				_logger.LogInformation("POLO4");
			}
		}

		await AnodeUOW.CommitTransaction();
	}

	public Task ReceiveStationImage(IFormFileCollection formFiles)
	{
		string imagesPath = _configuration.GetValueWithThrow<string>(ConfigDictionary.ImagesPath);

		string thumbnailsPath = _configuration.GetValueWithThrow<string>(ConfigDictionary.ThumbnailsPath);

		IEnumerable<Task> tasks = formFiles.ToList().Select(async formFile =>
		{
			FileInfo image = Shooting.GetImagePathFromFilename(imagesPath, formFile.Name);
			FileInfo thumbnail = Shooting.GetImagePathFromFilename(thumbnailsPath, formFile.Name);
			Directory.CreateDirectory(image.DirectoryName!);
			Directory.CreateDirectory(thumbnail.DirectoryName!);
			await using FileStream imageStream = new(image.FullName, FileMode.Create);
			await formFile.CopyToAsync(imageStream);
			Image savedImage = Image.FromFile(image.FullName);
			savedImage.Save(thumbnail.FullName, 0.2);
		});
		return Task.WhenAll(tasks);
	}

	public async Task ReceivePacketAlarm(List<DTOAlarmCycle> dtoAlarmCycles, string stationName, string cycleRID)
	{
		await AnodeUOW.StartTransaction();

		// insert Alarm Packet then associate AlarmCycle to Alarm Packet
		Packet alarmList = new AlarmList
		{
			StationCycleRID = cycleRID
		};
		await BuildPacket(alarmList);
		List<AlarmCycle> alarmCycles = dtoAlarmCycles.ConvertAll(dto => dto.ToModel());
		alarmCycles.ForEach(alarmCycle => {
			alarmCycle.AlarmListPacketID = alarmList.ID;
			AnodeUOW.AlarmCycle.Add(alarmCycle);
		});
		AnodeUOW.Commit();

		StationCycle stationCycle
				= await AnodeUOW.StationCycle.GetBy([cycle => cycle.RID == alarmList.StationCycleRID], withTracking: false);
		stationCycle.AssignPacket(alarmList);
		AnodeUOW.StationCycle.Update(stationCycle);
		AnodeUOW.Commit();
		await AnodeUOW.CommitTransaction();
	}
}