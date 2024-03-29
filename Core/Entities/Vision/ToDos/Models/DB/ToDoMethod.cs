using Core.Entities.Anodes.Models.DB;
using Core.Entities.StationCycles.Models.DB;
using Core.Entities.StationCycles.Models.DB.LoadableCycles.S1S2Cycles;
using Core.Entities.Vision.ToDos.Models.DB.ToSigns;
using Core.Entities.Vision.ToDos.Models.DTO;
using Core.Entities.Vision.ToDos.Models.DTO.ToSigns;
using Mapster;

namespace Core.Entities.Vision.ToDos.Models.DB;

public partial class ToDo
{
	public ToDo()
	{
	}

	public override DTOToDo ToDTO()
	{
		return this.Adapt<DTOToSign>();
	}

	private Anode ToAnode(StationCycle cycle)
	{
		TypeAdapterConfig.GlobalSettings.AllowImplicitSourceInheritance = true;
		TypeAdapterConfig.GlobalSettings.AllowImplicitDestinationInheritance = true;
		TypeAdapterConfig<(ToDo todo, StationCycle cycle), Anode>.NewConfig()
			.Map(dest => dest.CycleRID, src => src.todo.CycleRID)
			.Map(dest => dest.S1S2Cycle, src => src.cycle)
			.Map(dest => dest.S1S2CycleID, src => src.todo.CycleID);

		return (this, cycle).Adapt<Anode>();
	}
}