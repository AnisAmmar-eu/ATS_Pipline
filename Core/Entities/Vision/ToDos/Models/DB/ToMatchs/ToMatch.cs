using Core.Entities.Vision.ToDos.Models.DTO.ToMatchs;
using Core.Shared.Models.DB.Kernel.Interfaces;

namespace Core.Entities.Vision.ToDos.Models.DB.ToMatchs;

public partial class ToMatch : ToDo, IBaseEntity<ToMatch, DTOToMatch>
{
	public int InstanceMatchID { get; set; }
	public int NbActiveAlarms { get; set; }
	public bool HasPlug { get; set; }
}