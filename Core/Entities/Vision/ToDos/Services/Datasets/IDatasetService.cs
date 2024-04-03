using Core.Entities.Vision.Dictionaries;
using Core.Entities.Vision.ToDos.Models.DB.Datasets;
using Core.Entities.Vision.ToDos.Models.DTO.Datasets;
using Core.Shared.Services.Kernel.Interfaces;

namespace Core.Entities.Vision.ToDos.Services.Datasets;

public interface IDatasetService : IBaseEntityService<Dataset, DTODataset>
{
}