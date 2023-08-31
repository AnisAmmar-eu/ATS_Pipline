using Core.Entities.Journals.Models.DTOs;
using Microsoft.Extensions.Configuration;
using System.Configuration;

namespace Core.Entities.Journals.Models.DB
{
    public partial class Journal
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
