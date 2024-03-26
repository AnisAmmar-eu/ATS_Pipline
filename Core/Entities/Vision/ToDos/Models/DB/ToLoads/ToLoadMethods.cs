using Core.Entities.Vision.ToDos.Models.DTO.ToLoads;

namespace Core.Entities.Vision.ToDos.Models.DB.ToLoads;

public partial class ToLoad
{
	public ToLoad()
	{
	}

	public ToLoad(DTOToLoad dtoToLoad) : base(dtoToLoad)
	{
		LoadableCycleID = dtoToLoad.LoadableCycleID;
		if (dtoToLoad.LoadableCycle is not null)
			LoadableCycle = dtoToLoad.LoadableCycle;
	}

	public override DTOToLoad ToDTO()
	{
		return new(this);
	}
}