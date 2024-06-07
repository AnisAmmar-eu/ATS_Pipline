using Core.Entities.Vision.Testing.Models.DB;
using Mapster;

namespace Core.Entities.Vision.Testing.Models.DTO;

public partial class DTOStationTest
{
	public override StationTest ToModel() => this.Adapt<StationTest>();
}