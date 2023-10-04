using Core.Shared.Models.DB.Kernel;
using Core.Shared.Models.DB.Kernel.Interfaces;
using Core.Shared.Models.DTO.System.Logs;

namespace Core.Shared.Models.DB.System.Logs
{
	public partial class Log : BaseEntity, IBaseEntity<Log, DTOLog>
	{
		public Log()
		{
		}
		public Log(string server, string api, string controller, string function, string endpoint, int code, string value)
		{
			Server = server;
			Api = api;
			Controller = controller;
			Function = function;
			Endpoint = endpoint;
			Code = code;
			Value = value;
		}
		public Log(DTOLog dto)
		{
			Server = dto.Server;
			Api = dto.Api;
			Controller = dto.Controller;
			Function = dto.Function;
			Endpoint = dto.Endpoint;
			Code = dto.Code;
			Value = dto.Value;
		}

		public override DTOLog ToDTO()
		{
			return new DTOLog(this);
		}
	}
}
