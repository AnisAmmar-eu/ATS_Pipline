using Core.Entities.IOT.IOTDevices.Models.DB.ServerRules;
using Core.Shared.Models.DTO.Kernel.Interfaces;

namespace Core.Entities.IOT.IOTDevices.Models.DTO.ServerRules;

public partial class DTOServerRule : DTOIOTDevice, IDTO<ServerRule, DTOServerRule>
{
    bool Reinit { get; set;}
}