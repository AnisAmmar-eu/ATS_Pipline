using Core.Entities.IOT.IOTTags.Models.DB;
using Core.Entities.IOT.IOTTags.Models.DTO;
using Core.Shared.Repositories.Kernel.Interfaces;

namespace Core.Entities.IOT.IOTTags.Repositories;

public interface IIOTTagRepository : IBaseEntityRepository<IOTTag, DTOIOTTag>;