using Core.Entities.Anodes.Dictionaries;
using Core.Entities.Vision.ToDos.Models.DTO.ToSigns;
using Core.Shared.Models.DB.Kernel;
using Core.Shared.Models.DB.Kernel.Interfaces;

namespace Core.Entities.Vision.ToDos.Models.DB.ToSigns;

public partial class ToSign : ToDo, IBaseEntity<ToSign, DTOToSign>
{
}