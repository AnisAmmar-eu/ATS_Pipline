using Core.Entities.Vision.ToDos.Dictionaries;
using Core.Entities.Vision.ToDos.Models.DB.ToUnloads;
using Core.Entities.Vision.ToDos.Models.DTO.ToUnloads;
using Core.Shared.Data;
using Core.Shared.Exceptions;
using Core.Shared.Repositories.Kernel;

namespace Core.Entities.Vision.ToDos.Repositories.ToUnloads;

public class ToUnloadRepository : BaseEntityRepository<AnodeCTX, ToUnload, DTOToUnload>,
	IToUnloadRepository
{
	public ToUnloadRepository(AnodeCTX context) : base(context, [], [])
	{
	}
}