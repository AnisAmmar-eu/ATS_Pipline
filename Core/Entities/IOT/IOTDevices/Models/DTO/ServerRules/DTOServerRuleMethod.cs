using Core.Entities.IOT.IOTDevices.Models.DB.ServerRules;
using Mapster;

namespace Core.Entities.IOT.IOTDevices.Models.DTO.ServerRules;

public partial class DTOServerRule
{
	public DTOServerRule()
	{
	}

	public DTOServerRule(ServerRule ServerRule) : base(ServerRule)
	{
	}

	public override ServerRule ToModel() => this.Adapt<ServerRule>();
}