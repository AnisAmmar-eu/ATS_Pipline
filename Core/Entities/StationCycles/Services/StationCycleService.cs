using System.Configuration;
using System.Linq.Expressions;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using Core.Entities.Anodes.Models.DB;
using Core.Entities.Packets.Dictionaries;
using Core.Entities.Packets.Models.DB.Detections;
using Core.Entities.Packets.Models.DB.Shootings;
using Core.Entities.Packets.Models.Structs;
using Core.Entities.Packets.Services;
using Core.Entities.StationCycles.Models.DB;
using Core.Entities.StationCycles.Models.DB.MatchingCycles.S3S4Cycles;
using Core.Entities.StationCycles.Models.DB.SigningCycles.S1S2Cycles;
using Core.Entities.StationCycles.Models.DTO;
using Core.Entities.StationCycles.Models.Structs;
using Core.Entities.StationCycles.Repositories;
using Core.Shared.Dictionaries;
using Core.Shared.Exceptions;
using Core.Shared.Services.Kernel;
using Core.Shared.UnitOfWork.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Stemmer.Cvb;
using TwinCAT;
using TwinCAT.Ads;

namespace Core.Entities.StationCycles.Services;

public class StationCycleService : ServiceBaseEntity<IStationCycleRepository, StationCycle, DTOStationCycle>,
	IStationCycleService
{
	private readonly IConfiguration _configuration;
	private readonly IPacketService _packetService;

	public StationCycleService(IAnodeUOW anodeUOW, IPacketService packetService, IConfiguration configuration) :
		base(anodeUOW)
	{
		_packetService = packetService;
		_configuration = configuration;
	}

	public async Task<ReducedStationCycle?> GetMostRecentWithIncludes()
	{
		return (await AnodeUOW.StationCycle.GetAllWithIncludes(orderBy: query =>
			query.OrderByDescending(cycle => cycle.TS))).FirstOrDefault()?.Reduce();
	}

	public async Task<List<ReducedStationCycle>> GetAllRIDs()
	{
		return (await AnodeUOW.StationCycle.GetAll(withTracking: false,
			includes: new[] { "DetectionPacket", "ShootingPacket" })).ConvertAll(cycle => cycle.Reduce());
	}

	public async Task<List<StationCycle>> GetAllReadyToSent()
	{
		return await AnodeUOW.StationCycle.GetAllWithIncludes(new Expression<Func<StationCycle, bool>>[]
		{
			cycle => cycle.Status == PacketStatus.Completed
		}, withTracking: false);
	}

	public async Task<FileInfo> GetImagesFromIDAndCamera(int id, int camera)
	{
		StationCycle stationCycle = await AnodeUOW.StationCycle.GetById(id, includes: "ShootingPacket");
		if (stationCycle.ShootingPacket == null)
			throw new EntityNotFoundException("Pictures have not been yet assigned for this anode.");
		string? thumbnailsPath = _configuration.GetValue<string>("CameraConfig:ThumbnailsPath");
		if (thumbnailsPath == null)
			throw new ConfigurationErrorsException("Missing CameraConfig:ThumbnailsPath");
		return stationCycle.ShootingPacket.GetImagePathFromRoot(stationCycle.StationID, thumbnailsPath,
			stationCycle.AnodeType, camera);
	}

	public async Task UpdateDetectionWithMeasure(StationCycle stationCycle)
	{
		if (stationCycle.DetectionID == null)
			throw new InvalidOperationException("Station cycle with RID: " + stationCycle.RID +
			                                    " does not have a detection packet for measurement");
		Detection detection = (await AnodeUOW.Packet.GetById(stationCycle.DetectionID.Value) as Detection)!;
		AdsClient tcClient = new();
		tcClient.Connect(ADSUtils.AdsPort);
		if (!tcClient.IsConnected)
			throw new AdsException("Could not connect to TwinCat");
		uint handle = tcClient.CreateVariableHandle(ADSUtils.MeasurementVariable);
		MeasureStruct measure = tcClient.ReadAny<MeasureStruct>(handle);
		detection.AnodeSize = measure.AnodeSize;
		detection.MeasuredType = measure.AnodeType == 1 ? AnodeTypeDict.DX : AnodeTypeDict.D20;
		detection.IsSameType = measure.IsSameType;
		stationCycle.AnodeType = detection.MeasuredType;
		detection.Status = PacketStatus.Completed;
		await AnodeUOW.StartTransaction();
		AnodeUOW.Packet.Update(detection);
		AnodeUOW.StationCycle.Update(stationCycle);
		AnodeUOW.Commit();
		await AnodeUOW.CommitTransaction();
	}

	public async Task SendStationCycle(StationCycle stationCycle, string address)
	{
		using HttpClient httpClient = new();
		StringContent content =
			new(JsonSerializer.Serialize(stationCycle.ToDTO()), Encoding.UTF8, "application/json");
		HttpResponseMessage response = await httpClient.PostAsync($"{address}/apiServerReceive/stationCycles", content);
		if (response.IsSuccessStatusCode)
		{
			// Send the images, marking packets as sent and then cycle as sent.
			Task imagesTask = SendStationImages(stationCycle);
			await AnodeUOW.StartTransaction();
			stationCycle.Status = PacketStatus.Sent;
			if (stationCycle is S1S2Cycle s1S2Cycle)
				_packetService.MarkPacketAsSentFromStationCycle(s1S2Cycle.AnnouncementPacket);
			else
				_packetService.MarkPacketAsSentFromStationCycle(stationCycle.AnnouncementPacket);
			_packetService.MarkPacketAsSentFromStationCycle(stationCycle.DetectionPacket);
			_packetService.MarkPacketAsSentFromStationCycle(stationCycle.ShootingPacket);
			_packetService.MarkPacketAsSentFromStationCycle(stationCycle.AlarmListPacket);
			if (stationCycle is S3S4Cycle s3S4Cycle)
			{
				_packetService.MarkPacketAsSentFromStationCycle(s3S4Cycle.InFurnacePacket);
				_packetService.MarkPacketAsSentFromStationCycle(s3S4Cycle.OutFurnacePacket);
			}

			AnodeUOW.StationCycle.Update(stationCycle);
			AnodeUOW.Commit();
			await AnodeUOW.CommitTransaction();
			await imagesTask;
		}
	}

	public async Task ReceiveStationCycle(DTOStationCycle dtoStationCycle)
	{
		// DbContext operations should NOT be done concurrently. Hence why await in loop.
		await AnodeUOW.StartTransaction();
		StationCycle cycle = dtoStationCycle.ToModel();
		cycle.ID = 0;
		if (cycle is S1S2Cycle s1S2Cycle)
			s1S2Cycle.AnnouncementID = await _packetService.AddPacketFromStationCycle(s1S2Cycle.AnnouncementPacket);
		else
			cycle.AnnouncementID = await _packetService.AddPacketFromStationCycle(cycle.AnnouncementPacket);
		cycle.DetectionID = await _packetService.AddPacketFromStationCycle(cycle.DetectionPacket);
		cycle.ShootingID = await _packetService.AddPacketFromStationCycle(cycle.ShootingPacket);
		cycle.AlarmListID = await _packetService.AddPacketFromStationCycle(cycle.AlarmListPacket);
		if (cycle is S3S4Cycle s3S4Cycle)
		{
			s3S4Cycle.InFurnaceID = await _packetService.AddPacketFromStationCycle(s3S4Cycle.InFurnacePacket);
			s3S4Cycle.OutFurnaceID = await _packetService.AddPacketFromStationCycle(s3S4Cycle.OutFurnacePacket);
		}

		// Packets need to be commit before adding StationCycle
		AnodeUOW.Commit();
		await AnodeUOW.StationCycle.Add(cycle);
		AnodeUOW.Commit();
		await AssignCycleToAnode(cycle);

		AnodeUOW.Commit();
		await AnodeUOW.CommitTransaction();
	}

	public async Task ReceiveStationImage(IFormFileCollection formFiles)
	{
		string? imagesPath = _configuration.GetValue<string>("CameraConfig:ImagesPath");
		if (imagesPath == null)
			throw new ConfigurationErrorsException("Missing CameraConfig:ImagesPath");
		string? thumbnailsPath = _configuration.GetValue<string>("CameraConfig:ThumbnailsPath");
		if (thumbnailsPath == null)
			throw new ConfigurationErrorsException("Missing CameraConfig:ThumbnailsPath");
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
		await Task.WhenAll(tasks);
	}

	private async Task SendStationImages(StationCycle stationCycle)
	{
		string? imagesPath = _configuration.GetValue<string>("CameraConfig:ImagesPath");
		if (imagesPath == null)
			throw new ConfigurationErrorsException("Missing CameraConfig:ImagesPath");
		MultipartFormDataContent formData = new();
		formData.Headers.ContentType!.MediaType = "multipart/form-data";

		FileInfo image1 =
			stationCycle.ShootingPacket?.GetImagePathFromRoot(stationCycle.StationID, imagesPath,
				stationCycle.AnodeType, 1)!;
		if (image1.Exists)
		{
			StreamContent content1 = new(File.Open(image1.FullName, FileMode.Open));
			content1.Headers.ContentType = new MediaTypeHeaderValue("image/jpeg");
			formData.Add(content1, image1.Name, image1.Name);
		}

		FileInfo image2 =
			stationCycle.ShootingPacket?.GetImagePathFromRoot(stationCycle.StationID, imagesPath,
				stationCycle.AnodeType, 2)!;
		if (image2.Exists)
		{
			StreamContent content2 = new(File.Open(image2.FullName, FileMode.Open));
			content2.Headers.ContentType = new MediaTypeHeaderValue("image/jpeg");
			formData.Add(content2, image2.Name, image2.Name);
		}

		if (!formData.Any())
			return;

		using HttpClient httpClient = new();
		httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("multipart/form-data"));

		HttpResponseMessage response =
			await httpClient.PostAsync("https://localhost:7280/apiServerReceive/images", formData);
		if (!response.IsSuccessStatusCode)
			throw new HttpRequestException("Could not send images to the server: " + response.ReasonPhrase);
	}

	private async Task AssignCycleToAnode(StationCycle cycle)
	{
		if (cycle is S1S2Cycle s1S2Cycle)
		{
			Anode anode = Anode.Create(s1S2Cycle);
			await AnodeUOW.Anode.Add(anode);
		}
		// TODO Vision
	}
}