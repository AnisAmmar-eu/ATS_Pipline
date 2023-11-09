using Core.Entities.StationCycles.Models.DB;
using Core.Entities.StationCycles.Models.DTO;
using Core.Entities.StationCycles.Models.Structs;
using Core.Shared.Services.Kernel.Interfaces;
using Microsoft.AspNetCore.Http;

namespace Core.Entities.StationCycles.Services;

public interface IStationCycleService : IServiceBaseEntity<StationCycle, DTOStationCycle>
{
	public Task<ReducedStationCycle?> GetMostRecentWithIncludes();
	public Task<List<ReducedStationCycle>> GetAllRIDs();
	public Task<List<StationCycle>> GetAllReadyToSent();
	public Task<FileInfo> GetImagesFromIDAndCamera(int id, int camera);

	/// <summary>
	///     This function gets matching stats of stationCycles.
	/// </summary>
	/// <param name="period">TimeSpan from now to get stats from (eg. 24 hours)</param>
	/// <param name="stationID">Optional parameter to filter by stationID. If null, no filter is applied on stationID</param>
	/// <returns>
	///     Returns an int[] with 3 elements.
	///     int[0] = Number of Signed and Matched cycles. Both anodes are signed but only one need to be matched.
	///     int[1] = Number of Signed and NOT Matched cycles. Both anodes have to be signed.
	///     int[2] = Number of NON Signed cycles. If one of both pictures are signed, it still counts as non signed.
	///     int[3] = Percentage of match made on camera1.
	/// </returns>
	public Task<int[]> GetMatchingStats(TimeSpan period, int? stationID = null);

	public Task UpdateDetectionWithMeasure(StationCycle stationCycle);
	public Task SendStationCycles(List<StationCycle> stationCycles, string address);
	public Task SendStationImages(List<StationCycle> stationCycles);
	public Task ReceiveStationCycles(List<DTOStationCycle> dtoStationCycles);
	public Task ReceiveStationImage(IFormFileCollection formFiles);
}