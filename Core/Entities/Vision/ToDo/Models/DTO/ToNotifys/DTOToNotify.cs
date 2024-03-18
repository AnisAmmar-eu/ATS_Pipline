using Core.Entities.Anodes.Dictionaries;
using Core.Entities.Vision.ToDo.Models.DB.ToNotifys;
using Core.Shared.Models.DTO.Kernel;
using Core.Shared.Models.DTO.Kernel.Interfaces;

namespace Core.Entities.Vision.ToDo.Models.DTO.ToNotifys;

public partial class DTOToNotify :  DTOBaseEntity, IDTO<ToNotify, DTOToNotify>
{
    public string? SynchronisationKey { get; set; }
}