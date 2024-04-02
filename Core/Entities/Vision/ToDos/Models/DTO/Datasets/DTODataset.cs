using Core.Entities.StationCycles.Models.DB.LoadableCycles;
using Core.Entities.Vision.ToDos.Models.DB.Datasets;
using Core.Shared.Models.DTO.Kernel.Interfaces;

namespace Core.Entities.Vision.ToDos.Models.DTO.Datasets;

public partial class DTODataset : DTOToDo, IDTO<Dataset, DTODataset>
{
	public string SANfile { get; set; }
}