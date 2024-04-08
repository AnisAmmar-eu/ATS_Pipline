using Core.Entities.Vision.ToDos.Models.DTO.Datasets;
using Core.Shared.Models.DB.Kernel.Interfaces;

namespace Core.Entities.Vision.ToDos.Models.DB.Datasets;

public partial class Dataset : ToDo, IBaseEntity<Dataset, DTODataset>
{
    public int CameraID { get; set; }
}