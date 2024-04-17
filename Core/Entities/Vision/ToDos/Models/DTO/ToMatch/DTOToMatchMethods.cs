using Core.Entities.Vision.ToDos.Models.DB.ToMatchs;
using Mapster;

namespace Core.Entities.Vision.ToDos.Models.DTO.ToMatchs;

public partial class DTOToMatch
{
	public DTOToMatch()
	{
	}

	public override ToMatch ToModel() => this.Adapt<ToMatch>();
}