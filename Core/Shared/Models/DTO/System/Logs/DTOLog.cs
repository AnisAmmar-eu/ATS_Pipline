using Core.Shared.Models.DB.System.Logs;
using Core.Shared.Models.DTO.Kernel;
using Core.Shared.Models.DTO.Kernel.Interfaces;

namespace Core.Shared.Models.DTO.System.Logs
{
	public partial class DTOLog : DTOBaseEntity, IDTO<Log, DTOLog>
	{
		public string? Server { get; set; }
		public string? Api { get; set; }
		public string? Controller { get; set; }
		public string? Function { get; set; }
		public string? Endpoint { get; set; }
		public int? Code { get; set; }
		public string? Value { get; set; }
	}
}
