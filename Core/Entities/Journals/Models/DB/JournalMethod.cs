using Core.Entities.Journals.Models.DTOs;
using Microsoft.Extensions.Configuration;
using System.Configuration;
using Core.Shared.Models.DB.Kernel;
using Core.Shared.Models.DB.Kernel.Interfaces;

namespace Core.Entities.Journals.Models.DB
{
    public partial class Journal: BaseEntity, IBaseEntity<Journal, DTOJournal>
    {


        public Journal()
        {
            TS = DateTime.Now;
            Lu = 0;
        }

        public virtual DTOJournal ToDTO()
        {
            return new DTOJournal(this);
        }

    }
}
