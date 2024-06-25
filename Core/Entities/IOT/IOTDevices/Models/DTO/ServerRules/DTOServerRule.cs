using Core.Entities.IOT.IOTDevices.Models.DB.ServerRules;
using Core.Entities.IOTDevices.Dictionaries;
using Core.Shared.Models.DTO.Kernel.Interfaces;

namespace Core.Entities.IOT.IOTDevices.Models.DTO.ServerRules;

public partial class DTOServerRule : DTOIOTDevice, IDTO<ServerRule, DTOServerRule>
{
	new public string Type = IOTDevicesTypes.ServerRule;
	public bool Reinit { get; set; }
}