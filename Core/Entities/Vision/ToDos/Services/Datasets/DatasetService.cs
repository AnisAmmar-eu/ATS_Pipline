using Core.Entities.Vision.Dictionaries;
using Core.Entities.Vision.ToDos.Models.DB.Datasets;
using Core.Entities.Vision.ToDos.Models.DTO.Datasets;
using Core.Entities.Vision.ToDos.Repositories.Datasets;
using Core.Shared.Services.Kernel;
using Core.Shared.UnitOfWork.Interfaces;

namespace Core.Entities.Vision.ToDos.Services.Datasets;

public class ToDatasetService : BaseEntityService<IDatasetRepository, Dataset, DTODataset>,
	IDatasetService
{
	public ToDatasetService(IAnodeUOW anodeUOW) : base(anodeUOW)
	{
	}
}