using Core.Entities.Vision.ToDos.Models.DTO.ToMatchs;
using Mapster;

namespace Core.Entities.Vision.ToDos.Models.DB.ToMatchs;

public partial class ToMatch
{
	public ToMatch()
	{
	}

	public override DTOToMatch ToDTO()
	{
		return this.Adapt<DTOToMatch>();
	}
}