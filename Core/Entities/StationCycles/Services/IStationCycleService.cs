using Core.Entities.StationCycles.Models.DB;
using Core.Entities.StationCycles.Models.DTO;
using Core.Entities.StationCycles.Models.Structs;
using Core.Shared.Services.Kernel.Interfaces;

namespace Core.Entities.StationCycles.Services;

public interface IStationCycleService : IBaseEntityService<StationCycle, DTOStationCycle>
{
	public Task<ReducedStationCycle?> GetMostRecentWithIncludes();
	public Task<List<ReducedStationCycle>> GetAllRIDs();
	public Task<FileInfo> GetImagesFromIDAndCamera(int id, int camera);
}