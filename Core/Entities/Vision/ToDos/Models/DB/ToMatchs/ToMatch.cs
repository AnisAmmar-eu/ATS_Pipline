using Core.Entities.StationCycles.Models.DB.MatchableCycles;
using Core.Entities.Vision.Dictionaries;
using Core.Entities.Vision.ToDos.Models.DTO.ToMatchs;
using Core.Shared.Models.DB.Kernel.Interfaces;

namespace Core.Entities.Vision.ToDos.Models.DB.ToMatchs;

public partial class ToMatch : ToDo, IBaseEntity<ToMatch, DTOToMatch>
{
	public InstanceMatchID InstanceMatchID { get; set; }
}