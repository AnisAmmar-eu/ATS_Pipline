using Core.Entities.Vision.ToDos.Models.DB.ToUnloads;
using Core.Shared.Models.DTO.Kernel.Interfaces;

namespace Core.Entities.Vision.ToDos.Models.DTO.ToUnloads;

public partial class DTOToUnload : DTOToDo, IDTO<ToUnload, DTOToUnload>
{
	public string? SANfile { get; set; }
}