using Core.Entities.Vision.ToDos.Models.DB.Datasets;
using Core.Shared.Models.DTO.Kernel.Interfaces;
using Mapster;

namespace Core.Entities.Vision.ToDos.Models.DTO.Datasets;

public partial class DTODataset : DTOToDo, IDTO<Dataset, DTODataset>
{
	public DTODataset()
	{
	}

	public override Dataset ToModel()
	{
		return this.Adapt<Dataset>();
	}
}