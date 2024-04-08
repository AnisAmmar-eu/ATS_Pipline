using Core.Entities.Vision.ToDos.Models.DB.ToSigns;
using Core.Shared.Models.DTO.Kernel.Interfaces;

namespace Core.Entities.Vision.ToDos.Models.DTO.ToSigns;

public partial class DTOToSign :  DTOToDo, IDTO<ToSign, DTOToSign>
{
    public int CameraID { get; set; }
}