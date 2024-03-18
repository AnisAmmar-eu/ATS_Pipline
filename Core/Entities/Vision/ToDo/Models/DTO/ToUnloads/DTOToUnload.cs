using Core.Entities.Anodes.Dictionaries;
using Core.Entities.Vision.ToDo.Models.DB.ToUnloads;
using Core.Shared.Models.DTO.Kernel;
using Core.Shared.Models.DTO.Kernel.Interfaces;

namespace Core.Entities.Vision.ToDo.Models.DTO.ToUnloads;

public partial class DTOToUnload :  DTOBaseEntity, IDTO<ToUnload, DTOToUnload>
{
    public string? SynchronisationKey { get; set; }
}