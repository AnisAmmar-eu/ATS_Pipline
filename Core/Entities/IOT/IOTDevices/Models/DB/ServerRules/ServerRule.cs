using Core.Entities.IOT.IOTDevices.Models.DTO.ServerRules;
using Core.Shared.Models.DB.Kernel.Interfaces;

namespace Core.Entities.IOT.IOTDevices.Models.DB.ServerRules;

public partial class ServerRule : IOTDevice, IBaseEntity<ServerRule, DTOServerRule>
{
	public bool Reinit { get; set; }
}