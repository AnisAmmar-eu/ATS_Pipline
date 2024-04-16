using Core.Entities.IOT.IOTDevices.Models.DB.BackgroundServices.Matchs;
using Core.Entities.IOT.IOTDevices.Models.DTO;
using Core.Entities.IOT.IOTDevices.Models.DTO.BackgroundServices.Matchs;
using Core.Shared.Repositories.Kernel.Interfaces;

namespace Core.Entities.IOT.IOTDevices.Repositories;

public interface IMatchRepository : IBaseEntityRepository<Match, DTOMatch>;