using Core.Entities.Anodes.Models.DB.AnodesD20;
using Core.Entities.Anodes.Dictionaries;

namespace Core.Entities.Anodes.Models.DTO.AnodesD20;

public partial class DTOAnodeD20
{
	public DTOAnodeD20()
	{
		AnodeType = AnodeTypes.D20;
	}

	public DTOAnodeD20(AnodeD20 anodeD20) : base(anodeD20)
	{
		AnodeType = AnodeTypes.D20;
	}

	public override AnodeD20 ToModel()
	{
		return new(this);
	}
}