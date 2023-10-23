using System.Collections.Specialized;
using System.Linq.Expressions;
using System.Net.Http.Json;
using System.Text;
using Core.Entities.Packets.Dictionaries;
using Core.Entities.Packets.Models.DB.Detections;
using Core.Entities.Packets.Models.Structs;
using Core.Entities.Packets.Services;
using Core.Entities.StationCycles.Models.DB;
using Core.Entities.StationCycles.Models.DB.S1S2Cycles;
using Core.Entities.StationCycles.Models.DB.S3S4Cycles;
using Core.Entities.StationCycles.Models.DTO;
using Core.Entities.StationCycles.Models.DTO.S1S2Cycles;
using Core.Entities.StationCycles.Models.DTO.S3S4Cycles;
using Core.Entities.StationCycles.Repositories;
using Core.Shared.Dictionaries;
using Core.Shared.Services.Kernel;
using Core.Shared.UnitOfWork.Interfaces;
using Newtonsoft.Json;
using TwinCAT;
using TwinCAT.Ads;

namespace Core.Entities.StationCycles.Services;

public class StationCycleService : ServiceBaseEntity<IStationCycleRepository, StationCycle, DTOStationCycle>,
	IStationCycleService
{
	private readonly IPacketService _packetService;

	public StationCycleService(IAnodeUOW anodeUOW, IPacketService packetService) : base(anodeUOW)
	{
		_packetService = packetService;
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

	public async Task<List<StationCycle>> GetAllReadyToSent()
	{
		return await AnodeUOW.StationCycle.GetAll(filters: new Expression<Func<StationCycle, bool>>[]
			{
				cycle => cycle.Status == PacketStatus.Completed
			}, withTracking: false,
			includes: new[]
			{
				"AnnouncementPacket", "DetectionPacket", "ShootingPacket", "AlarmListPacket", "InFurnacePacket",
				"OutFurnacePacket"
			});
	}

	public async Task SendStationCycles(List<StationCycle> stationCycles, string address)
	{
		if (!stationCycles.Any())
			return;
		using HttpClient httpClient = new();
		httpClient.DefaultRequestHeaders.Add("accept", "*/*");
		StringContent content =
			new(JsonConvert.SerializeObject(stationCycles.ConvertAll(cycle => cycle.ToDTO())), Encoding.UTF8,
				"application/json");
		var response = await httpClient.PostAsync($"{address}/api/receive/station-cycle", content);
		if (response.IsSuccessStatusCode)
		{
			await AnodeUOW.StartTransaction();
			stationCycles.ForEach(cycle =>
			{
				cycle.Status = PacketStatus.Sent;
				if (cycle is S1S2Cycle s1S2Cycle)
					_packetService.MarkPacketAsSentFromStationCycle(s1S2Cycle.AnnouncementPacket);
				else
					_packetService.MarkPacketAsSentFromStationCycle(cycle.AnnouncementPacket);
				_packetService.MarkPacketAsSentFromStationCycle(cycle.DetectionPacket);
				_packetService.MarkPacketAsSentFromStationCycle(cycle.ShootingPacket);
				_packetService.MarkPacketAsSentFromStationCycle(cycle.AlarmListPacket);
				if (cycle is S3S4Cycle s3S4Cycle)
				{
					_packetService.MarkPacketAsSentFromStationCycle(s3S4Cycle.InFurnacePacket);
					_packetService.MarkPacketAsSentFromStationCycle(s3S4Cycle.OutFurnacePacket);
				}

				AnodeUOW.StationCycle.Update(cycle);
			});
			AnodeUOW.Commit();
			await AnodeUOW.CommitTransaction();
		}
	}

	public async Task ReceiveStationCycles(List<DTOStationCycle> dtoStationCycles)
	{
		IEnumerable<Task> tasks = dtoStationCycles.Select(async dto =>
		{
			await AnodeUOW.StartTransaction();
			StationCycle cycle = dto.ToModel();
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
		});
		await Task.WhenAll(tasks);
		AnodeUOW.Commit();
		await AnodeUOW.CommitTransaction();
	}
}