using Core.Entities.Vision.FileSettings.Models.DB;
using Core.Entities.Vision.FileSettings.Models.DTO;
using Core.Shared.Data;
using Core.Shared.Repositories.Kernel;

namespace Core.Entities.Vision.FileSettings.Repositories;

public class FileSettingRepository : BaseEntityRepository<AnodeCTX, FileSetting, DTOFileSetting>, IFileSettingRepository
{
	public FileSettingRepository(AnodeCTX context) : base(context)
	{
	}
}