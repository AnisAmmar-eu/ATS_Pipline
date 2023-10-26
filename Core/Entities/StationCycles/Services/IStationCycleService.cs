using Core.Entities.StationCycles.Models.DB;
using Core.Entities.StationCycles.Models.DTO;
using Core.Entities.StationCycles.Models.Structs;
using Core.Shared.Services.Kernel.Interfaces;

namespace Core.Entities.StationCycles.Services;

public interface IStationCycleService : IServiceBaseEntity<StationCycle, DTOStationCycle>
{
	public Task<ReducedStationCycle?> GetMostRecentWithIncludes();
	public Task<List<ReducedStationCycle>> GetAllRIDs();
	public Task<List<StationCycle>> GetAllReadyToSent();
	public Task<byte[]> GetImagesFromIDAndCamera(int id, int camera);
	public Task UpdateDetectionWithMeasure(StationCycle stationCycle);
	public Task SendStationCycles(List<StationCycle> stationCycles, string address);
	public Task ReceiveStationCycles(List<DTOStationCycle> dtoStationCycles);
}