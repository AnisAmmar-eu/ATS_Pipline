using Core.Entities.Packets.Models.DB.Shootings;
using Core.Entities.StationCycles.Models.DB;
using Core.Entities.StationCycles.Models.DTO;
using Core.Entities.StationCycles.Models.Structs;
using Core.Entities.StationCycles.Repositories;
using Core.Shared.Configuration;
using Core.Shared.Dictionaries;
using Core.Shared.Exceptions;
using Core.Shared.Services.Kernel;
using Core.Shared.UnitOfWork.Interfaces;
using Microsoft.Extensions.Configuration;
using Core.Entities.StationCycles.Dictionaries;
using Core.Entities.StationCycles.Models.DB.MatchableCycles;

namespace Core.Entities.StationCycles.Services;

public class StationCycleService :
	BaseEntityService<IStationCycleRepository, StationCycle, DTOStationCycle>,
	IStationCycleService
{
	private readonly IConfiguration _configuration;

	public StationCycleService(IAnodeUOW anodeUOW, IConfiguration configuration) :
		base(anodeUOW)
	{
		_configuration = configuration;
	}

	public async Task<DTOReducedStationCycle?> GetMostRecentWithIncludes()
	{
		return (await _anodeUOW.StationCycle.GetAllWithIncludes(orderBy: query =>
			query.OrderByDescending(cycle => cycle.TS)))
			.FirstOrDefault()
			?.Reduce();
	}

	public async Task<List<DTOReducedStationCycle>> GetAllRIDs()
	{
		return (await _anodeUOW.StationCycle
			.GetAll(withTracking: false, includes: [nameof(StationCycle.Shooting1Packet)]))
			.ConvertAll(cycle => cycle.Reduce());
	}

	public async Task<FileInfo> GetImagesFromIDAndCamera(int id, int camera)
	{
		string includeProperty = (camera == 1) ? nameof(StationCycle.Shooting1Packet) : nameof(StationCycle.Shooting2Packet);
		StationCycle stationCycle = await _anodeUOW.StationCycle.GetById(id, includes: includeProperty);

		Shooting? shootingPacket = ((camera == 1) ? stationCycle.Shooting1Packet : stationCycle.Shooting2Packet)
			?? throw new EntityNotFoundException("Pictures have not been yet assigned for this anode.");
		string thumbnailsPath = _configuration.GetValueWithThrow<string>(ConfigDictionary.ThumbnailsPath);
		string extenstion = _configuration.GetValueWithThrow<string>(ConfigDictionary.CameraExtension);

		return Shooting.GetImagePathFromRoot(
			shootingPacket.StationCycleRID,
			stationCycle.StationID,
			thumbnailsPath,
			stationCycle.AnodeType,
			camera,
			extenstion);
	}

	public async Task<int[]> GetSignMatchResults(int? stationId)
	{
		DateTimeOffset last24Hours = DateTimeOffset.UtcNow.AddHours(-24);

		List<StationCycle> stationCycles = await _anodeUOW.StationCycle
			.GetAll([sc => sc.TSFirstShooting >= last24Hours]);

		if (stationId is not null)
			stationCycles = stationCycles.Where(sc => sc.StationID == stationId).ToList();

		int notSignedCount = stationCycles.Count(
			sc => sc.SignStatus1 != SignMatchStatus.Ok && sc.SignStatus2 != SignMatchStatus.Ok);

		int signNotMatchedCount = stationCycles.Count(sc => (sc is MatchableCycle matchableCycle)
			&& (matchableCycle.MatchingCamera1 != SignMatchStatus.Ok
				|| matchableCycle.MatchingCamera2 != SignMatchStatus.Ok)
			&& (sc.SignStatus1 != SignMatchStatus.Ok && sc.SignStatus2 != SignMatchStatus.Ok));

		int matchedCount = stationCycles.Count(sc => (sc is MatchableCycle matchableCycle)
			&& (matchableCycle.MatchingCamera1 == SignMatchStatus.Ok
				|| matchableCycle.MatchingCamera2 == SignMatchStatus.Ok));

		return [notSignedCount, signNotMatchedCount, matchedCount];
	}

	public async Task<int[]> GetMainAndSecondHoleStatus(int? stationId)
	{
		List<StationCycle> stationCycles = await _anodeUOW.StationCycle.GetAll();
		if (stationId is not null)
			stationCycles = stationCycles.Where(sc => sc.StationID == stationId).ToList();

		int mainHoleCount = stationCycles.Count(sc => (sc is MatchableCycle matchableCycle)
			&& (matchableCycle.MatchingCamera1 == SignMatchStatus.Ok));

		int secondHoleCount = stationCycles.Count(sc => (sc is MatchableCycle matchableCycle)
			&& (matchableCycle.MatchingCamera2 == SignMatchStatus.Ok));

		return [mainHoleCount, secondHoleCount];
	}

	public async Task<int[]> GetAnodeCounterByStation()
	{
		int[] stationCounts = new int[5];

		List<StationCycle> stationCycles = await _anodeUOW.StationCycle.GetAll();

		foreach (int stationId in Enumerable.Range(1, 5))
			stationCounts[stationId - 1] = stationCycles.Count(sc => sc.StationID == stationId);

		return stationCounts;
	}

	public async Task<int[]> GetAnodeCounterByAnodeType()
	{
		List<StationCycle> stationCycles = await _anodeUOW.StationCycle.GetAll();

		int dx = stationCycles.Count(sc => sc.AnodeType == "01");
		int d20 = stationCycles.Count(sc => sc.AnodeType == "02");

		return [dx, d20];
	}
}