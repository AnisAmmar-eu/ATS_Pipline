using Core.Entities.Vision.ToDos.Models.DB.ToMatchs;
using Core.Entities.Vision.ToDos.Models.DTO.ToMatchs;
using Core.Shared.Data;
using Core.Shared.Repositories.Kernel;

namespace Core.Entities.Vision.ToDos.Repositories.ToMatchs;

public class ToMatchRepository :
	BaseEntityRepository<AnodeCTX, ToMatch, DTOToMatch>,
	IToMatchRepository
{
	public ToMatchRepository(AnodeCTX context) : base(context, [], [])
	{
	}
}