using Core.Entities.Vision.ToDos.Models.DTO.Datasets;
using Mapster;

namespace Core.Entities.Vision.ToDos.Models.DB.Datasets;

public partial class Dataset
{
	public Dataset()
	{
	}

	public override DTODataset ToDTO()
	{
		return this.Adapt<DTODataset>();
	}
}