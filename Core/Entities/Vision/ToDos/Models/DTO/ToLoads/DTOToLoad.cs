using Core.Entities.Vision.ToDos.Models.DB.ToLoads;
using Core.Shared.Models.DTO.Kernel.Interfaces;

namespace Core.Entities.Vision.ToDos.Models.DTO.ToLoads;

public partial class DTOToLoad : DTOToDo, IDTO<ToLoad, DTOToLoad>
{
    public int CameraID { get; set; }
}