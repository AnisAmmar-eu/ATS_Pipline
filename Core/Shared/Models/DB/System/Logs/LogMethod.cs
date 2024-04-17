using Core.Shared.Dictionaries;
using Core.Shared.Models.DTO.System.Logs;

namespace Core.Shared.Models.DB.System.Logs;

public partial class Log
{
	public Log()
	{
		HasBeenSent = Station.IsServer;
	}

	public Log(
		string server,
		string username,
		string api,
		string controller,
		string function,
		string endpoint,
		int code,
		string value,
		int stationID)
	{
		HasBeenSent = Station.IsServer;
		Server = server;
		Username = username;
		Api = api;
		Controller = controller;
		Function = function;
		Endpoint = endpoint;
		Code = code;
		Value = value;
		StationID = stationID;
	}

	public Log(DTOLog dto)
	{
		Server = dto.Server;
		Username = dto.Username;
		Api = dto.Api;
		Controller = dto.Controller;
		Function = dto.Function;
		Endpoint = dto.Endpoint;
		Code = dto.Code;
		Value = dto.Value;
		HasBeenSent = dto.HasBeenSent;
		StationID = dto.StationID;
	}

	public override DTOLog ToDTO() => new(this);
}