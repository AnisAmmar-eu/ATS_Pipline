using Core.Entities.Vision.ToDos.Models.DB.Datasets;
using Core.Entities.Vision.ToDos.Models.DTO.Datasets;
using Core.Shared.Data;
using Core.Shared.Repositories.Kernel;

namespace Core.Entities.Vision.ToDos.Repositories.Datasets;

public class DatasetRepository :
	BaseEntityRepository<AnodeCTX, Dataset, DTODataset>,
	IDatasetRepository
{
	public DatasetRepository(AnodeCTX context) : base(context, [], [])
	{
	}
}