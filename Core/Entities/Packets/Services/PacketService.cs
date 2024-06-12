using System.Net;
using System.Net.Http.Json;
using System.Reflection;
using System.Text;
using System.Text.Json;
using Core.Entities.Alarms.AlarmsCycle.Models.DB;
using Core.Entities.Alarms.AlarmsCycle.Models.DTO;
using Core.Entities.IOT.Dictionaries;
using Core.Entities.Packets.Dictionaries;
using Core.Entities.Packets.Models.DB;
using Core.Entities.Packets.Models.DB.AlarmLists;
using Core.Entities.Packets.Models.DB.MetaDatas;
using Core.Entities.Packets.Models.DB.Shootings;
using Core.Entities.Packets.Models.DTO;
using Core.Entities.Packets.Models.DTO.Shootings;
using Core.Entities.Packets.Repositories;
using Core.Entities.StationCycles.Models.DB;
using Core.Entities.Vision.ToDos.Models.DB.ToSigns;
using Core.Entities.Vision.ToDos.Services.ToMatchs;
using Core.Shared.Configuration;
using Core.Shared.Dictionaries;
using Core.Shared.Exceptions;
using Core.Shared.Models.ApiResponses;
using Core.Shared.Services.Kernel;
using Core.Shared.UnitOfWork.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

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
		return (await _anodeUOW.Packet
			.GetByWithThrow(
				[packet => packet is Shooting],
				orderBy: query => query.OrderByDescending(packet => packet.ID)) as Shooting
			?? throw new EntityNotFoundException())
			.ToDTO();
	}

	//Same logic as above
	//This function checks oldest not sent packet timestamp for monitoring
	public async Task<DateTimeOffset> GetOldestNotSentTimestamp()
	{
		try
		{
			return (await _anodeUOW.Packet
				.GetByWithThrow([packet => packet is Shooting && packet.Status != PacketStatus.Sent]) as Shooting)
				?.ShootingTS
				?? DateTimeOffset.Now;
		}
		catch (EntityNotFoundException)
		{
			return DateTimeOffset.Now;
		}
	}

	public async Task<FileInfo> GetThumbnailFromCycleRIDAndCamera(int shootingID, int cameraID)
	{
		Shooting shooting = await _anodeUOW.Packet
			.GetByWithThrow([packet => packet.ID == shootingID]) as Shooting
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

	public async Task<FileInfo> GetImageFromCycleRIDAndCamera(int shootingID, int cameraID)
	{
		Shooting shooting = await _anodeUOW.Packet
			.GetByWithThrow([packet => packet.ID == shootingID]) as Shooting
			?? throw new EntityNotFoundException("Found a packet but it is not a shooting one");

		string imagesPath = _configuration.GetValueWithThrow<string>(ConfigDictionary.ImagesPath);
		string extension = _configuration.GetValueWithThrow<string>(ConfigDictionary.CameraExtension);

		return Shooting.GetImagePathFromRoot(
			shooting.StationCycleRID,
			Station.ID,
			imagesPath,
			shooting.AnodeType,
			cameraID,
			extension);
	}

	public async Task<DTOPacket> BuildPacket(Packet packet)
	{
		await _anodeUOW.StartTransaction();

		await packet.Create(_anodeUOW);

		await packet.Build(_anodeUOW);

		await _anodeUOW.CommitTransaction();
		return packet.ToDTO();
	}

	public async Task SendCompletedPackets()
	{
		string extension = _configuration.GetValueWithThrow<string>(ConfigDictionary.CameraExtension);
		IEnumerable<Packet> packets
			= await _anodeUOW.Packet.GetAll(
				[packet => packet.Status == PacketStatus.Completed],
				query => query.OrderByDescending(packet => packet.ID),
				withTracking: false);
		if (!packets.Any())
			return;

		foreach (Packet packet in packets)
		{
			try
			{
				using HttpClient http = new();

				if (packet is AlarmList alarmLists)
				{
					// Get all alarmCycle
					List<DTOAlarmCycle> dtoAlarmCycles = [];
					try
					{
						dtoAlarmCycles = (await _anodeUOW.AlarmCycle
							.GetAll([alarmCycle => alarmCycle.AlarmListPacketID == alarmLists.ID], withTracking: false))
							.ConvertAll(alarmCycle => alarmCycle.ToDTO());
					}
					catch (Exception e)
					{
						_logger.LogError("No AlarmCycle: {e}", e);
					}

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
				else
				{
					if (packet is Shooting shooting)
					{
						string imagesPath = _configuration.GetValueWithThrow<string>(ConfigDictionary.ImagesPath);
						string thumbnailsPath = _configuration.GetValueWithThrow<string>(ConfigDictionary.ThumbnailsPath);
						await shooting.SendImages(imagesPath, thumbnailsPath, extension, _logger);
					}

					HttpResponseMessage response = await http.PostAsJsonAsync(
						$"{ITApisDict.ServerReceiveAddress}/apiServerReceive/{Station.Name}/packets",
						packet.ToDTO(),
						ApiResponse.JsonOptions);
					if (response.StatusCode != HttpStatusCode.OK)
					{
						throw new("Send packet to server failed with status code:"
							+ $" {response.StatusCode.ToString()}\nReason: {response.ReasonPhrase}");
					}
				}

				await _anodeUOW.Packet.ExecuteUpdateByIdAsync(
					packet,
					setters => setters.SetProperty(packet => packet.Status, PacketStatus.Sent));
			}
			catch (Exception e)
			{
				_logger.LogError("Error when sending packet: {e}", e);
			}
		}
	}

	public async Task ReceivePacket(DTOPacket dtoPacket, string stationName)
	{
		Packet packet = dtoPacket.ToModel();
		packet.ID = 0;
		packet.Status = PacketStatus.Sent;
		await _anodeUOW.StartTransaction();
		await _anodeUOW.Packet.Add(packet);
		_anodeUOW.Commit();
		try
		{
			StationCycle stationCycle
				= await _anodeUOW.StationCycle.GetByWithThrow([cycle => cycle.RID == packet.StationCycleRID], withTracking: false);

			if (packet is Shooting shooting)
			{
				_logger.LogError("Packet shooting update");

				if (stationCycle.AnodeType is AnodeTypeDict.Undefined)
					stationCycle.AnodeType = shooting.AnodeType;

				if (shooting.Cam01Status == 1)
					stationCycle.Shooting1Packet = shooting;
				else
					stationCycle.Shooting2Packet = shooting;

				await _anodeUOW.ToSign.Add(ToSign.ShootingToSign(shooting, stationCycle));
				_anodeUOW.Commit();
			}
			else if (packet is MetaData metaData)
			{
				_logger.LogInformation("MetaData packet received");
				stationCycle.AssignPacket(metaData);
				if (stationCycle.AnodeType is AnodeTypeDict.Undefined)
					stationCycle.AnodeType = AnodeTypeDict.AnodeTypeIntToString(metaData.AnodeType_MD);

				stationCycle.Picture1Status = metaData.Cam01Status;
				stationCycle.Picture2Status = metaData.Cam02Status;
				stationCycle.SerialNumber = metaData.GetSerialNumber();

				if (stationCycle.CanMatch())
				{
					_logger.LogInformation("Creating ToMatch");
					foreach (int instanceMatchID in await ToMatchService.GetMatchInstance(
						stationCycle.AnodeType,
						stationCycle.StationID,
						_anodeUOW))
					{
						await _anodeUOW.ToMatch.Add(new(stationCycle, instanceMatchID));
					}

					_anodeUOW.Commit();
				}
			}
			else
			{
				_logger.LogInformation("Other packet received");
				stationCycle.AssignPacket(packet);
			}

			_anodeUOW.StationCycle.Update(stationCycle);
			_anodeUOW.Commit();
		}
		catch (EntityNotFoundException)
		{
			StationCycle stationCycle = StationCycle.Create($"S{packet.StationCycleRID[0].ToString()}");
			stationCycle.StationID = int.Parse(packet.StationCycleRID[0].ToString());
			stationCycle.RID = packet.StationCycleRID;

			if (packet is Shooting shooting)
			{
				_logger.LogError("Packet shooting first time");

				stationCycle.AnodeType = shooting.AnodeType;
				stationCycle.TSFirstShooting = shooting.ShootingTS;
				if (shooting.Cam01Status == 1)
					stationCycle.Shooting1Packet = shooting;
				else
					stationCycle.Shooting2Packet = shooting;
			}
			else if (packet is MetaData metaData)
			{
				_logger.LogInformation("MetaData packet received");
				stationCycle.AssignPacket(metaData);
				stationCycle.AnodeType = AnodeTypeDict.AnodeTypeIntToString(metaData.AnodeType_MD);
				stationCycle.Picture1Status = metaData.Cam01Status;
				stationCycle.Picture2Status = metaData.Cam02Status;
				stationCycle.SerialNumber = metaData.GetSerialNumber();
			}
			else
			{
				_logger.LogInformation("Other packet received");
				stationCycle.AssignPacket(packet);
			}

			await _anodeUOW.StationCycle.Add(stationCycle);
			_anodeUOW.Commit();

			if (packet is Shooting shoot)
			{
				await _anodeUOW.ToSign.Add(ToSign.ShootingToSign(shoot, stationCycle));
				_anodeUOW.Commit();
			}
		}

		await _anodeUOW.CommitTransaction();
	}

	public Task ReceiveStationImage(IFormFileCollection formFiles, bool isImage)
	{
		string imagesPath = _configuration.GetValueWithThrow<string>(ConfigDictionary.ImagesPath);
		string thumbnailsPath = _configuration.GetValueWithThrow<string>(ConfigDictionary.ThumbnailsPath);
		IEnumerable<Task> tasks = formFiles.ToList().Select(async formFile => {
			string path = isImage ? imagesPath : thumbnailsPath;
			FileInfo image = Shooting.GetImagePathFromFilename(path, formFile.Name);
			Directory.CreateDirectory(image.DirectoryName!);
			await using FileStream imageStream = new(image.FullName, FileMode.Create);
			await formFile.CopyToAsync(imageStream);
		});
		return Task.WhenAll(tasks);
	}

	public async Task ReceivePacketAlarm(List<DTOAlarmCycle> dtoAlarmCycles, string stationName, string cycleRID)
	{
		await _anodeUOW.StartTransaction();

		// insert Alarm Packet then associate AlarmCycle to Alarm Packet
		AlarmList alarmList = new() {
			StationCycleRID = cycleRID
		};
		await BuildPacket(alarmList);
		dtoAlarmCycles.ForEach(dto => dto.AlarmListPacketID = alarmList.ID);
		List<AlarmCycle> alarmCycles = dtoAlarmCycles.ConvertAll(dto => dto.ToModel());
		alarmCycles.ForEach(alarmCycle => alarmCycle.AlarmList = alarmList);
		await _anodeUOW.AlarmCycle.AddRange(alarmCycles);
		_anodeUOW.Commit();

		try
		{
			StationCycle stationCycle
				= await _anodeUOW.StationCycle.GetByWithThrow([cycle => cycle.RID == cycleRID], withTracking: false);
			stationCycle.AssignPacket(alarmList);
			_anodeUOW.StationCycle.Update(stationCycle);
			_anodeUOW.Commit();
		}
		catch (EntityNotFoundException)
		{
			StationCycle stationCycle = StationCycle.Create(cycleRID[0].ToString());
			stationCycle.StationID = Station.StationNameToID(cycleRID[0].ToString());
			stationCycle.RID = cycleRID;
			stationCycle.AssignPacket(alarmList);
			await _anodeUOW.StationCycle.Add(stationCycle);
			_anodeUOW.Commit();
		}
		catch (Exception)
		{
			throw;
		}

		await _anodeUOW.CommitTransaction();
	}
}