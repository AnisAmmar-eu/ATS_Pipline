using Core.Entities.Packets.Models.DB.Detections;
using Core.Entities.Packets.Models.Structs;
using Core.Entities.StationCycles.Services;
using Core.Shared.Dictionaries;
using Microsoft.Extensions.DependencyInjection;

namespace Core.Shared.Services.Notifications;

public class AssignNotification : BaseNotification<Detection, DetectionStruct>
{
	public static string? ImagesPath { get; set; }
	public static string? ThumbnailsPath { get; set; }

	public static async Task<AssignNotification> Create(dynamic ads)
	{
		return await CreateSub<AssignNotification>(
			ads,
			ADSUtils.DetectionRemove,
			ADSUtils.CloseCycle,
			ADSUtils.DetectionToRead);
	}

	protected override Task AddElement(IServiceProvider services, Detection entity)
	{
		IStationCycleService stationCycleService = services.GetRequiredService<IStationCycleService>();
		if (ImagesPath is null || ThumbnailsPath is null)
			throw new ArgumentException("ImagesPath or ThumbnailsPath have NOT been initialised");

		return stationCycleService.AssignStationCycle(entity, ImagesPath, ThumbnailsPath);
	}
}