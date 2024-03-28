using Core.Entities.Vision.ToDos.Dictionaries;
using Core.Entities.Vision.ToDos.Models.DB.Datasets;
using Core.Entities.Vision.ToDos.Models.DTO.Datasets;
using Core.Shared.Repositories.Kernel.Interfaces;

namespace Core.Entities.Vision.ToDos.Repositories.Datasets;

public interface IDatasetRepository : IBaseEntityRepository<Dataset, DTODataset>
{
}