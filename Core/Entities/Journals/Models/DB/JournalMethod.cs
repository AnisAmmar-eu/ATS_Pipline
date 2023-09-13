using Core.Entities.Journals.Models.DTO;
using Core.Shared.Models.DB.Kernel;
using Core.Shared.Models.DB.Kernel.Interfaces;

namespace Core.Entities.Journals.Models.DB;

public partial class Journal : BaseEntity, IBaseEntity<Journal, DTOJournal>
{
    public Journal()
    {
        TS = DateTime.Now;
        IsRead = 0;
    }

    public Journal(DTOJournal dtoJournal)
    {
        ID = dtoJournal.ID;
        TS = (DateTimeOffset)dtoJournal.TS!;
        TS0 = dtoJournal.TS0;
        TS1 = dtoJournal.TS1;
        Status0 = dtoJournal.Status0;
        Status1 = dtoJournal.Status1;
        IDAlarm = dtoJournal.IDAlarm;
        Alarm = dtoJournal.Alarm;
        Station = dtoJournal.Station;
        IsRead = dtoJournal.IsRead;
    }

    public DTOJournal ToDTO(string? languageRID = null)
    {
        return new DTOJournal(this, languageRID);
    }
}