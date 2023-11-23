using Core.Entities.Vision.FileSettings.Models.DB;
using Core.Entities.Vision.FileSettings.Models.DTO;
using Core.Shared.Repositories.Kernel.Interfaces;

namespace Core.Entities.Vision.FileSettings.Repositories;

public interface IFileSettingRepository : IRepositoryBaseEntity<FileSetting, DTOFileSetting>;