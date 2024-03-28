using Core.Entities.Vision.ToDos.Dictionaries;
using Core.Entities.Vision.ToDos.Models.DB.ToUnloads;
using Core.Entities.Vision.ToDos.Models.DTO.ToUnloads;
using Core.Shared.Services.Kernel.Interfaces;

namespace Core.Entities.Vision.ToDos.Services.ToUnloads;

public interface IToUnloadService : IBaseEntityService<ToUnload, DTOToUnload>
{
}