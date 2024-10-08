using Core.Entities.Vision.ToDos.Models.DTO.ToNotifys;
using Core.Shared.Models.DB.Kernel;
using Core.Shared.Models.DB.Kernel.Interfaces;

namespace Core.Entities.Vision.ToDos.Models.DB.ToNotifys;

public partial class ToNotify : BaseEntity, IBaseEntity<ToNotify, DTOToNotify>
{
	public string? SynchronisationKey { get; set; }

	public DateTimeOffset ShootingTS { get; set; }

	public string? Path { get; set; }

	public int StationID { get; set; }

	public string SerialNumber { get; set; }
}