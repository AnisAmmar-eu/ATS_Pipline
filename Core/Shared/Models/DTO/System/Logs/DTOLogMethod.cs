using Core.Shared.Models.DB.System.Logs;
using Core.Shared.Models.DTO.Kernel;
using Core.Shared.Models.DTO.Kernel.Interfaces;

namespace Core.Shared.Models.DTO.System.Logs;

public partial class DTOLog : DTOBaseEntity, IDTO<Log, DTOLog>
{
	public DTOLog()
	{
	}

	public DTOLog(Log log)
	{
		ID = log.ID;
		Server = log.Server;
		Api = log.Api;
		Controller = log.Controller;
		Function = log.Function;
		Endpoint = log.Endpoint;
		Code = log.Code;
		Value = log.Value;
		TS = log.TS;
	}

	public override Log ToModel()
	{
		return new Log(this);
	}
}