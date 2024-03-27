using Core.Entities.IOT.IOTDevices.Models.DB.Stations;
using Core.Shared.Models.DTO.Kernel.Interfaces;

namespace Core.Entities.IOT.IOTDevices.Models.DTO.Stations;

public partial class DTOStation : DTOIOTDevice, IDTO<Station, DTOStation>;