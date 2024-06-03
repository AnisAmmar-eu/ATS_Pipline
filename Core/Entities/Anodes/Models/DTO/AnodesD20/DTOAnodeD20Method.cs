using Core.Entities.Anodes.Dictionaries;
using Core.Entities.Anodes.Models.DB.AnodesD20;
using Mapster;

namespace Core.Entities.Anodes.Models.DTO.AnodesD20;

public partial class DTOAnodeD20
{
	public DTOAnodeD20()
	{
		AnodeType = AnodeTypes.D20;
	}

	public override AnodeD20 ToModel() => this.Adapt<AnodeD20>();
}