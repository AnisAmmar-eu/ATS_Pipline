using Core.Entities.Anodes.Models.DB.AnodesD20;

namespace Core.Entities.Anodes.Models.DTO.AnodesD20;

public partial class DTOAnodeD20
{
	public DTOAnodeD20(AnodeD20 anodeD20) : base(anodeD20)
	{
	}

	public override AnodeD20 ToModel()
	{
		return new(this);
	}
}