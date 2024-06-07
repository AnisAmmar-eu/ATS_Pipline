using Core.Entities.DebugsModes.Models.DTO;
using Mapster;

namespace Core.Entities.DebugsModes.Models.DB;

public partial class DebugMode
{
	public DebugMode()
	{
	}

	public override DTODebugMode ToDTO() => this.Adapt<DTODebugMode>();
}