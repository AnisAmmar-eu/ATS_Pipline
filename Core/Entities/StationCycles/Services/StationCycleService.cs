using System.Configuration;
using Core.Entities.StationCycles.Models.DB;
using Core.Entities.StationCycles.Models.DTO;
using Core.Entities.StationCycles.Models.Structs;
using Core.Entities.StationCycles.Repositories;
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
			.GetAll(withTracking: false, includes: [nameof(StationCycle.DetectionPacket), nameof(StationCycle.ShootingPacket)]))
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

		string? thumbnailsPath = _configuration.GetValue<string>("CameraConfig:ThumbnailsPath");
		if (thumbnailsPath is null)
			throw new ConfigurationErrorsException("Missing CameraConfig:ThumbnailsPath");

		return stationCycle.ShootingPacket
			.GetImagePathFromRoot(stationCycle.StationID, thumbnailsPath, stationCycle.AnodeType, camera);
	}
}