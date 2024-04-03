using Core.Entities.Vision.Dictionaries;
using Core.Entities.Vision.ToDos.Models.DB.ToNotifys;
using Core.Entities.Vision.ToDos.Models.DTO.ToNotifys;
using Core.Shared.Repositories.Kernel.Interfaces;

namespace Core.Entities.Vision.ToDos.Repositories.ToNotifys;

public interface IToNotifyRepository : IBaseEntityRepository<ToNotify, DTOToNotify>
{
}