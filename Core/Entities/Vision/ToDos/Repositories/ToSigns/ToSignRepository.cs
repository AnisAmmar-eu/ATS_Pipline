using Core.Entities.Vision.Dictionaries;
using Core.Entities.Vision.ToDos.Models.DB.ToSigns;
using Core.Entities.Vision.ToDos.Models.DTO.ToSigns;
using Core.Shared.Data;
using Core.Shared.Exceptions;
using Core.Shared.Repositories.Kernel;

namespace Core.Entities.Vision.ToDos.Repositories.ToSigns;

public class ToSignRepository : BaseEntityRepository<AnodeCTX, ToSign, DTOToSign>,
	IToSignRepository
{
	public ToSignRepository(AnodeCTX context) : base(context, [], [])
	{
	}
}