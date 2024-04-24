using Core.Entities.IOT.IOTDevices.Models.DTO.BackgroundServices.Signs;
using Core.Shared.Models.DB.Kernel.Interfaces;

namespace Core.Entities.IOT.IOTDevices.Models.DB.BackgroundServices.Signs;

public partial class Sign : BackgroundService, IBaseEntity<Sign, DTOSign>;