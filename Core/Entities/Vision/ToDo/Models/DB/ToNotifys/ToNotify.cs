using Core.Entities.Anodes.Dictionaries;
using Core.Entities.Vision.ToDo.Models.DTO.ToNotifys;
using Core.Shared.Models.DB.Kernel;
using Core.Shared.Models.DB.Kernel.Interfaces;

namespace Core.Entities.Vision.ToDo.Models.DB.ToNotifys;

public partial class ToNotify : BaseEntity, IBaseEntity<ToNotify, DTOToNotify>
{
    public string? SynchronisationKey { get; set; }
}