using Core.Entities.Anodes.Dictionaries;
using Core.Entities.Vision.ToDo.Models.DTO.ToSigns;
using Core.Shared.Models.DB.Kernel;
using Core.Shared.Models.DB.Kernel.Interfaces;

namespace Core.Entities.Vision.ToDo.Models.DB.ToSigns;

public partial class ToSign : BaseEntity, IBaseEntity<ToSign, DTOToSign>
{
    public int CycleID { get; set; }
    public int CycleRID { get; set; }
    public int CameraID { get; set; }
    public int StationID { get; set; }
    public string AnodeType { get; set; }
    public DateTimeOffset? ShootingTS { get; set; }
}