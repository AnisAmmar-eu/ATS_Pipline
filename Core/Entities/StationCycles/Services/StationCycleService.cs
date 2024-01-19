using Core.Entities.Packets.Models.DB.Shootings;
using Core.Entities.StationCycles.Models.DB;
using Core.Entities.StationCycles.Models.DTO;
using Core.Entities.StationCycles.Models.Structs;
using Core.Entities.StationCycles.Repositories;
using Core.Shared.Configuration;
using Core.Shared.Exceptions;
using Core.Shared.Services.Kernel;
using Core.Shared.UnitOfWork.Interfaces;
using Microsoft.Extensions.Configuration;

namespace Core.Entities.StationCycles.Services;

public class StationCycleService : BaseEntityService<IStationCycleRepository, StationCycle, DTOStationCycle>,
	IStationCycleService
{
	private readonly IConfiguration _configuration;

	public StationCycleService(IAnodeUOW anodeUOW, IConfiguration configuration) :
		base(anodeUOW)
	{
		_configuration = configuration;
	}

	public async Task<ReducedStationCycle?> GetMostRecentWithIncludes()
	{
		return (await AnodeUOW.StationCycle.GetAllWithIncludes(orderBy: query =>
			query.OrderByDescending(cycle => cycle.TS)))
			.FirstOrDefault()
			?.Reduce();
	}

	public async Task<List<ReducedStationCycle>> GetAllRIDs()
	{
		return (await AnodeUOW.StationCycle
			.GetAll(withTracking: false, includes: [nameof(StationCycle.ShootingPacket)]))
			.ConvertAll(cycle => cycle.Reduce());
	}

	public async Task<FileInfo> GetImagesFromIDAndCamera(int id, int camera)
	{
		StationCycle stationCycle
			= await AnodeUOW.StationCycle.GetById(
				id,
				includes: nameof(StationCycle.ShootingPacket));
		if (stationCycle.ShootingPacket is null)
			throw new EntityNotFoundException("Pictures have not been yet assigned for this anode.");

		string thumbnailsPath = _configuration.GetValueWithThrow<string>("CameraConfig:ThumbnailsPath");
		string extenstion = _configuration.GetValueWithThrow<string>("CameraConfig:Extension");

		return Shooting.GetImagePathFromRoot(
			stationCycle.ShootingPacket.StationCycleRID,
			stationCycle.StationID,
			thumbnailsPath,
			stationCycle.AnodeType,
			camera,
			extenstion);
	}
}