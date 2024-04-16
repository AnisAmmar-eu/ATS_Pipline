using Core.Entities.IOT.IOTDevices.Models.DB.BackgroundServices.Signs;
using Core.Entities.IOT.IOTDevices.Models.DTO.BackgroundServices.Signs;
using Core.Shared.Repositories.Kernel.Interfaces;

namespace Core.Entities.IOT.IOTDevices.Repositories;

public interface ISignRepository : IBaseEntityRepository<Sign, DTOSign>;