using Core.Entities.IOT.IOTDevices.Models.DTO.ServerRules;
using Mapster;

namespace Core.Entities.IOT.IOTDevices.Models.DB.ServerRules;

public partial class ServerRule
{
	public ServerRule()
	{
	}

	public ServerRule(DTOServerRule dtoServerRule) : base(dtoServerRule)
	{
	}

	public override DTOServerRule ToDTO() => this.Adapt<DTOServerRule>();
}