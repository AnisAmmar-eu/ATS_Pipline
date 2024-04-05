using System.Net;
using System.Text.Json;
using Core.Entities.IOT.Dictionaries;
using Core.Entities.IOT.IOTDevices.Models.DB.Stations;
using Core.Entities.IOT.IOTDevices.Models.DTO.ServerRules;
using Core.Entities.IOT.IOTDevices.Models.DTO.Stations;
using Core.Entities.IOT.IOTTags.Models.DB;
using Core.Shared.Dictionaries;
using Core.Shared.Models.ApiResponses;
using Core.Shared.Models.TwinCat;
using Core.Shared.UnitOfWork;
using Core.Shared.UnitOfWork.Interfaces;
using Microsoft.Extensions.Logging;
using TwinCAT.Ads;

namespace Core.Entities.IOT.IOTDevices.Models.DB.ServerRules;

public partial class ServerRule
{
	public ServerRule()
	{
	}

	public ServerRule(DTOServerRule dtoServerRule) : base(dtoServerRule)
	{
	}

	public override DTOServerRule ToDTO()
	{
		return new(this);
	}
}