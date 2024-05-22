using Core.Entities.Vision.ToDos.Models.DB.ToMatchs;
using Core.Shared.Models.DTO.Kernel.Interfaces;

namespace Core.Entities.Vision.ToDos.Models.DTO.ToMatchs;

public partial class DTOToMatch : DTOToDo, IDTO<ToMatch, DTOToMatch>
{
	public int InstanceMatchID { get; set; }
	public int NbActiveAlarms { get; set; }
	public bool HasPlug { get; set; }
}