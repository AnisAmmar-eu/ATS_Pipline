using Core.Entities.Vision.ToDos.Models.DB.ToNotifys;
using Core.Shared.Models.DTO.Kernel;
using Core.Shared.Models.DTO.Kernel.Interfaces;

namespace Core.Entities.Vision.ToDos.Models.DTO.ToNotifys;

public partial class DTOToNotify :  DTOBaseEntity, IDTO<ToNotify, DTOToNotify>
{
    public string? SynchronisationKey { get; set; }
}