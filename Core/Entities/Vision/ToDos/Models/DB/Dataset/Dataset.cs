using Core.Entities.StationCycles.Models.DB.LoadableCycles;
using Core.Entities.Vision.Dictionaries;
using Core.Entities.Vision.ToDos.Models.DTO.Datasets;
using Core.Shared.Models.DB.Kernel.Interfaces;

namespace Core.Entities.Vision.ToDos.Models.DB.Datasets;

public partial class Dataset : ToDo, IBaseEntity<Dataset, DTODataset>
{
	public string SANfile { get; set; }
}