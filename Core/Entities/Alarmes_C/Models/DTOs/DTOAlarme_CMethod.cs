using Core.Entities.Alarmes_C.Models.DB;
using Core.Shared.Models.DTOs.Kernel;
using Core.Shared.Models.DTOs.Kernel.Interfaces;

namespace Core.Entities.Alarmes_C.Models.DTOs;

public partial class DTOAlarme_C : DTOBaseEntity, IDTO<Alarme_C, DTOAlarme_C>
{
   public DTOAlarme_C() {}

   public Alarme_C ToModel()
   {
      return new();
   }
}