using Core.Entities.Vision.ToDos.Models.DB.ToUnloads;
using Core.Entities.Vision.ToDos.Models.DTO.ToUnloads;
using Core.Shared.Repositories.Kernel.Interfaces;

namespace Core.Entities.Vision.ToDos.Repositories.ToUnloads;

public interface IToUnloadRepository : IBaseEntityRepository<ToUnload, DTOToUnload>
{
}