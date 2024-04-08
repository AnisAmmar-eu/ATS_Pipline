using Core.Entities.IOT.IOTDevices.Models.DB.ITApiStations;
using Core.Entities.IOT.IOTDevices.Models.DTO.ITApis;
using Core.Shared.Models.DTO.Kernel.Interfaces;

namespace Core.Entities.IOT.IOTDevices.Models.DTO.ITApiStations;

public partial class DTOITApiStation : DTOITApi, IDTO<ITApiStation, DTOITApiStation>;