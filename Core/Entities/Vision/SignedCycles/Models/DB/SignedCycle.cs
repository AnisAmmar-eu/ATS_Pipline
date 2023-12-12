using Core.Entities.Vision.SignedCycles.Dictionaries;
using Core.Entities.Vision.SignedCycles.Models.DTO;
using Core.Shared.Models.DB.Kernel;
using Core.Shared.Models.DB.Kernel.Interfaces;

namespace Core.Entities.Vision.SignedCycles.Models.DB;

public partial class SignedCycle : BaseEntity, IBaseEntity<SignedCycle, DTOSignedCycle>
{
	public DateTimeOffset CycleTS { get; set; }
	public DataSetID DataSetID { get; set; }
	public string SAN1Path { get; set; } = string.Empty;
	public string SAN2Path { get; set; } = string.Empty;
}