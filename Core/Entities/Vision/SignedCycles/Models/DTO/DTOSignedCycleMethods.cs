using Core.Entities.Vision.SignedCycles.Models.DB;

namespace Core.Entities.Vision.SignedCycles.Models.DTO;

public partial class DTOSignedCycle
{
	public DTOSignedCycle()
	{
	}

	public DTOSignedCycle(SignedCycle signedCycle) : base(signedCycle)
	{
		CycleTS = signedCycle.CycleTS;
		DataSetID = signedCycle.DataSetID;
		SAN1Path = signedCycle.SAN1Path;
		SAN2Path = signedCycle.SAN2Path;
	}

	public override SignedCycle ToModel()
	{
		return new(this);
	}
}