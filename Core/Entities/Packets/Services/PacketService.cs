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
using Core.Entities.Packets.Models.DB.MetaDatas;
using Core.Entities.Packets.Models.DB.Shootings;
using Core.Entities.Packets.Models.DTO;
using Core.Entities.Packets.Models.DTO.AlarmLists;
using Core.Entities.Packets.Models.DTO.Shootings;
using Core.Entities.Packets.Repositories;
using Core.Entities.StationCycles.Dictionaries;
using Core.Entities.StationCycles.Models.DB;
using Core.Entities.Vision.ToDos.Models.DB.ToMatchs;
using Core.Entities.Vision.ToDos.Models.DB.ToSigns;
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

	//Same logic as above
	//This function checks oldest not sent packet timestamp for monitoring
	public async Task<DateTimeOffset> GetOldestNotSentTimestamp()
	{
		try
		{
			return (await AnodeUOW.Packet
				.GetBy([packet => packet is Shooting && packet.Status != PacketStatus.Sent]) as Shooting)
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
		Shooting shooting = await AnodeUOW.Packet
			.GetBy([packet => packet.ID == shootingID]) as Shooting
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
		Shooting shooting = await AnodeUOW.Packet
			.GetBy([packet => packet.ID == shootingID]) as Shooting
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
		await AnodeUOW.StartTransaction();

		await packet.Create(AnodeUOW);

		await packet.Build(AnodeUOW);

		await AnodeUOW.CommitTransaction();
		return packet.ToDTO();
	}

	public async Task SendCompletedPackets()
	{
		string extension = _configuration.GetValueWithThrow<string>(ConfigDictionary.CameraExtension);
		IEnumerable<Packet> packets
			= await AnodeUOW.Packet.GetAll(
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
						dtoAlarmCycles = (await AnodeUOW.AlarmCycle
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

				await AnodeUOW.Packet.ExecuteUpdateByIdAsync(
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
		await AnodeUOW.StartTransaction();
		await AnodeUOW.Packet.Add(packet);
		AnodeUOW.Commit();
		try
		{
			StationCycle stationCycle
				= await AnodeUOW.StationCycle.GetBy([cycle => cycle.RID == packet.StationCycleRID], withTracking: false);

			if (packet is Shooting shooting)
			{
				if (stationCycle.AnodeType is "")
					stationCycle.AnodeType = shooting.AnodeType;

				if (shooting.Cam01Status == 1)
					stationCycle.Shooting1Packet = shooting;
				else
					stationCycle.Shooting2Packet = shooting;

				await AnodeUOW.ToSign.Add(ToSign.ShootingToSign(shooting));
				AnodeUOW.Commit();
			}
			else if(packet is MetaData metaData)
			{
				stationCycle.AssignPacket(metaData);
				if (stationCycle.AnodeType is "")
					stationCycle.AnodeType = AnodeTypeDict.AnodeTypeIntToString(metaData.AnodeType_MD);

				stationCycle.Picture1Status = metaData.Cam01Status;
				stationCycle.Picture2Status = metaData.Cam02Status;

				if (stationCycle.CanMatch())
				{
					_logger.LogInformation("Creating ToMatch");
					await AnodeUOW.ToMatch.Add(new (stationCycle));
					AnodeUOW.Commit();
				}
			}
			else
			{
				stationCycle.AssignPacket(packet);
			}

			AnodeUOW.StationCycle.Update(stationCycle);
			AnodeUOW.Commit();
		}
		catch (EntityNotFoundException)
		{
			StationCycle stationCycle = StationCycle.Create(stationName);
			stationCycle.StationID = Station.StationNameToID(stationName);
			stationCycle.RID = packet.StationCycleRID;
			if (packet is Shooting shooting)
			{
				stationCycle.AnodeType = shooting.AnodeType;
				stationCycle.TSFirstShooting = shooting.ShootingTS;
				if (shooting.Cam01Status == 1)
					stationCycle.Shooting1Packet = shooting;
				else
					stationCycle.Shooting2Packet = shooting;

				await AnodeUOW.ToSign.Add(ToSign.ShootingToSign(shooting));
				AnodeUOW.Commit();
			}
			else if (packet is MetaData metaData)
			{
				_logger.LogInformation("MetaData packet received");
				stationCycle.AssignPacket(metaData);
				stationCycle.AnodeType = AnodeTypeDict.AnodeTypeIntToString(metaData.AnodeType_MD);
				stationCycle.Picture1Status = metaData.Cam01Status;
				stationCycle.Picture2Status = metaData.Cam02Status;
			}
			else
			{
				_logger.LogInformation("Other packet received");
				stationCycle.AssignPacket(packet);
			}

			await AnodeUOW.StationCycle.Add(stationCycle);
			AnodeUOW.Commit();
		}

		await AnodeUOW.CommitTransaction();
	}

	public Task ReceiveStationImage(IFormFileCollection formFiles, bool isImage)
	{
		string imagesPath = _configuration.GetValueWithThrow<string>(ConfigDictionary.ImagesPath);
		string thumbnailsPath = _configuration.GetValueWithThrow<string>(ConfigDictionary.ThumbnailsPath);

		IEnumerable<Task> tasks = formFiles.ToList().Select(async formFile =>
		{
			string path = isImage ? imagesPath : thumbnailsPath;
			FileInfo image = Shooting.GetImagePathFromFilename(path, formFile.Name);
			Directory.CreateDirectory(image.DirectoryName!);

			await using FileStream imageStream = new(image.FullName, FileMode.Create);
			await formFile.CopyToAsync(imageStream);
			_logger.LogInformation("Saving image 1 imageName: {name}", image.FullName);
		});
		return Task.WhenAll(tasks);
	}

	public async Task ReceivePacketAlarm(List<DTOAlarmCycle> dtoAlarmCycles, string stationName, string cycleRID)
	{
		await AnodeUOW.StartTransaction();

		// insert Alarm Packet then associate AlarmCycle to Alarm Packet
		AlarmList alarmList = new()
		{
			StationCycleRID = cycleRID
		};
		await BuildPacket(alarmList);
		dtoAlarmCycles.ForEach(dto => dto.AlarmListPacketID = alarmList.ID);
		List<AlarmCycle> alarmCycles = dtoAlarmCycles.ConvertAll(dto => dto.ToModel());
		alarmCycles.ForEach(alarmCycle => alarmCycle.AlarmList = alarmList);
		await AnodeUOW.AlarmCycle.AddRange(alarmCycles);
		AnodeUOW.Commit();

		try
		{
			StationCycle stationCycle
				= await AnodeUOW.StationCycle.GetBy([cycle => cycle.RID == cycleRID], withTracking: false);
			stationCycle.AssignPacket(alarmList);
			AnodeUOW.StationCycle.Update(stationCycle);
			AnodeUOW.Commit();
		}
		catch (EntityNotFoundException)
		{
			StationCycle stationCycle = StationCycle.Create(stationName);
			stationCycle.StationID = Station.StationNameToID(stationName);
			stationCycle.RID = cycleRID;
			stationCycle.AssignPacket(alarmList);
			await AnodeUOW.StationCycle.Add(stationCycle);
			AnodeUOW.Commit();
		}
		catch (Exception)
		{
			throw;
		}

		await AnodeUOW.CommitTransaction();
	}
}