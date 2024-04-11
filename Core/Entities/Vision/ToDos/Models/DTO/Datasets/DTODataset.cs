using Core.Entities.Vision.ToDos.Models.DB.Datasets;
using Core.Shared.Models.DTO.Kernel.Interfaces;

namespace Core.Entities.Vision.ToDos.Models.DTO.Datasets;

public partial class DTODataset : DTOToDo, IDTO<Dataset, DTODataset>
{
	public int GateID { get; set; }
	public int CandidateGateID { get; set; }
	public int CameraID { get; set; }
}