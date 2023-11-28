using Core.Entities.Packets.Models.DB.Detections;
using Core.Entities.StationCycles.Models.DB;
using Core.Entities.StationCycles.Models.DTO;
using Core.Entities.StationCycles.Models.Structs;
using Core.Shared.Services.Kernel.Interfaces;
using Microsoft.AspNetCore.Http;

namespace Core.Entities.StationCycles.Services;

public interface IStationCycleService : IBaseEntityService<StationCycle, DTOStationCycle>
{
	public Task<ReducedStationCycle?> GetMostRecentWithIncludes();
	public Task<List<ReducedStationCycle>> GetAllRIDs();
	public Task<List<StationCycle>> GetAllReadyToSent();
	public Task<FileInfo> GetImagesFromIDAndCamera(int id, int camera);
	public Task AssignStationCycle(Detection detection, string imagesPath, string thumbnailsPath);
	public Task UpdateDetectionWithMeasure(StationCycle stationCycle);
	public Task SendStationCycle(StationCycle stationCycle, string address);
	public Task ReceiveStationCycle(DTOStationCycle dtoStationCycle);
	public Task ReceiveStationImage(IFormFileCollection formFiles);
}