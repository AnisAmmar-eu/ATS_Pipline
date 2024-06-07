using Core.Entities.DebugsModes.Models.DB;
using Mapster;

namespace Core.Entities.DebugsModes.Models.DTO;

public partial class DTODebugMode
{
	public DTODebugMode()
	{
	}

	public override DebugMode ToModel() => this.Adapt<DebugMode>();
}