using Core.Entities.Vision.Dictionaries;
using Core.Entities.Vision.ToDos.Models.DB.ToSigns;
using Core.Entities.Vision.ToDos.Models.DTO.ToSigns;
using Core.Shared.Repositories.Kernel.Interfaces;

namespace Core.Entities.Vision.ToDos.Repositories.ToSigns;

public interface IToSignRepository : IBaseEntityRepository<ToSign, DTOToSign>
{
}