using Core.Shared.Dictionaries;
using Core.Shared.Models.DB.System.Logs;

namespace Core.Shared.Models.DTO.System.Logs;

public partial class DTOLog
{
	public DTOLog()
	{
		HasBeenSent = Station.IsServer;
	}

	public DTOLog(Log log)
	{
		ID = log.ID;
		TS = log.TS;
		Server = log.Server;
		Username = log.Username;
		Api = log.Api;
		Controller = log.Controller;
		Function = log.Function;
		Endpoint = log.Endpoint;
		Code = log.Code;
		Value = log.Value;
		HasBeenSent = log.HasBeenSent;
		StationID = log.StationID;
	}

	public override Log ToModel()
	{
		return new Log(this);
	}
}