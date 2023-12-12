using Core.Entities.Vision.SignedCycles.Models.DTO;

namespace Core.Entities.Vision.SignedCycles.Models.DB;

public partial class SignedCycle
{
	public SignedCycle()
	{
	}

	public SignedCycle(DTOSignedCycle dtoSignedCycle) : base(dtoSignedCycle)
	{
		CycleTS = dtoSignedCycle.CycleTS;
		DataSetID = dtoSignedCycle.DataSetID;
		SAN1Path = dtoSignedCycle.SAN1Path;
		SAN2Path = dtoSignedCycle.SAN2Path;
	}

	public override DTOSignedCycle ToDTO()
	{
		return new(this);
	}
}