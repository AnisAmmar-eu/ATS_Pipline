using Core.Entities.Vision.Dictionaries;
using Core.Entities.Vision.ToDos.Models.DTO.ToUnloads;
using Core.Shared.Models.DB.Kernel.Interfaces;

namespace Core.Entities.Vision.ToDos.Models.DB.ToUnloads;

public partial class ToUnload : ToDo, IBaseEntity<ToUnload, DTOToUnload>
{
	public InstanceMatchID InstanceMatchID { get; set; }
}