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
		return await CreateSub<AssignNotification>(ads, ADSUtils.DetectionRemove, ADSUtils.CloseCycle,
			ADSUtils.DetectionToRead);
	}

	protected override async Task AddElement(IServiceProvider services, Detection detection)
	{
		IStationCycleService stationCycleService = services.GetRequiredService<IStationCycleService>();
		if (ImagesPath == null || ThumbnailsPath == null)
			throw new ArgumentException("ImagesPath or ThumbnailsPath have NOT been initialised");
		await stationCycleService.AssignStationCycle(detection, ImagesPath, ThumbnailsPath);
	}
}