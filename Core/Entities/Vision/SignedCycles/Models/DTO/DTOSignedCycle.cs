using Core.Entities.Vision.SignedCycles.Dictionaries;
using Core.Entities.Vision.SignedCycles.Models.DB;
using Core.Shared.Models.DTO.Kernel;
using Core.Shared.Models.DTO.Kernel.Interfaces;

namespace Core.Entities.Vision.SignedCycles.Models.DTO;

public partial class DTOSignedCycle : DTOBaseEntity, IDTO<SignedCycle, DTOSignedCycle>
{
	public DateTimeOffset CycleTS { get; set; }
	public DataSetID DataSetID { get; set; }
	public string SAN1Path { get; set; } = string.Empty;
	public string SAN2Path { get; set; } = string.Empty;
}