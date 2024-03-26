using Core.Entities.Anodes.Dictionaries;
using Core.Entities.Vision.ToDos.Models.DB.ToSigns;
using Core.Shared.Models.DTO.Kernel;
using Core.Shared.Models.DTO.Kernel.Interfaces;

namespace Core.Entities.Vision.ToDos.Models.DTO.ToSigns;

public partial class DTOToSign :  DTOToDo, IDTO<ToSign, DTOToSign>
{
    public int CycleID { get; set; }
    public int CycleRID { get; set; }
    public int CameraID { get; set; }
    public int StationID { get; set; }
    public string? AnodeType { get; set; }
    public DateTimeOffset? ShootingTS { get; set; }
}