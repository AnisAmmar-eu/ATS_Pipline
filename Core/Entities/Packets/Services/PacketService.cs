using System.Net;
using System.Text;
using System.Text.Json;
using Core.Entities.IOT.Dictionaries;
using Core.Entities.Packets.Dictionaries;
using Core.Entities.Packets.Models.DB;
using Core.Entities.Packets.Models.DB.Shootings;
using Core.Entities.Packets.Models.DTO;
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

	public async Task<FileInfo> GetImageFromIDAndCamera(int shootingID, int cameraID)
	{
		Shooting shooting = await AnodeUOW.Packet.GetById(shootingID) as Shooting
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
			= await AnodeUOW.Packet.GetAll([packet => packet.Status == PacketStatus.Completed], withTracking: false);
		if (!packets.Any())
			return;

		await AnodeUOW.StartTransaction();
		foreach (Packet packet in packets)
		{
			try
			{
				using HttpClient http = new();
				if (packet is Shooting shooting)
					await shooting.SendImages(imagesPath, extension);

				StringContent content
					= new(JsonSerializer.Serialize(packet.ToDTO(), ApiResponse.JsonOptions), Encoding.UTF8, "application/json");
				HttpResponseMessage response = await http.PostAsync(
					$"{ITApisDict.ServerReceiveAddress}/apiServerReceive/{Station.Name}/packets",
					content);
				if (response.StatusCode != HttpStatusCode.OK)
				{
					throw new("Send packet to server failed with status code:"
						+ $" {response.StatusCode.ToString()}\nReason: {response.ReasonPhrase}");
				}

				packet.Status = PacketStatus.Sent;
				AnodeUOW.Packet.Update(packet);
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
		Packet packet = dtoPacket.ToModel();
		packet.ID = 0;
		await AnodeUOW.StartTransaction();
		await AnodeUOW.Packet.Add(packet);
		AnodeUOW.Commit();
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
				// Every "orphan" packet is aggregated to this stationCycle.
				List<Packet> packets
					= await AnodeUOW.Packet
						.GetAll(
							[packet1 => packet1.StationCycleRID == stationCycle.RID, packet2 => packet2.ID != shooting.ID],
							withTracking: false);
				packets.ForEach(stationCycle.AssignPacket);
				await AnodeUOW.StationCycle.Add(stationCycle);
				AnodeUOW.Commit();
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
}