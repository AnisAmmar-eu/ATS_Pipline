using Core.Entities.Anodes.Dictionaries;
using Core.Entities.Vision.ToDo.Models.DTO.ToUnloads;
using Core.Shared.Models.DB.Kernel;
using Core.Shared.Models.DB.Kernel.Interfaces;

namespace Core.Entities.Vision.ToDo.Models.DB.ToUnloads;

public partial class ToUnload : BaseEntity, IBaseEntity<ToUnload, DTOToUnload>
{
    public string? SynchronisationKey { get; set; }
}