using Core.Entities.Vision.ToDos.Models.DB.ToNotifys;
using Core.Entities.Vision.ToDos.Models.DTO.ToNotifys;
using Core.Shared.Services.Kernel.Interfaces;

namespace Core.Entities.Vision.ToDos.Services.ToNotifys;

public interface IToNotifyService : IBaseEntityService<ToNotify, DTOToNotify>
{
}