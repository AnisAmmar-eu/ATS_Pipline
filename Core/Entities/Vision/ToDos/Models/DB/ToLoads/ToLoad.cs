using Core.Entities.Vision.ToDos.Models.DTO.ToLoads;
using Core.Shared.Models.DB.Kernel.Interfaces;

namespace Core.Entities.Vision.ToDos.Models.DB.ToLoads;

public partial class ToLoad : ToDo, IBaseEntity<ToLoad, DTOToLoad>
{
	public int InstanceMatchID { get; set; }
	public int CameraID { get; set; }
}