using Core.Entities.IOT.IOTDevices.Models.DB.ServerRules;

namespace Core.Entities.IOT.IOTDevices.Models.DTO.ServerRules;

public partial class DTOServerRule
{
	public DTOServerRule()
	{
	}

	public DTOServerRule(ServerRule ServerRule) : base(ServerRule)
	{
	}

	public override ServerRule ToModel()
	{
		return new(this);
	}
}