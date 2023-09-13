using Core.Entities.Journals.Models.DB;
using Core.Shared.Models.DTOs.Kernel;
using Core.Shared.Models.DTOs.Kernel.Interfaces;

namespace Core.Entities.Journals.Models.DTO;

public partial class DTOJournal : DTOBaseEntity, IDTO<Journal, DTOJournal>
{
    public DTOJournal()
    {
        TS = DateTime.Now;
        IsRead = 0;
    }


    public DTOJournal(Journal journal, string languageRID)
    {
        ID = journal.ID;
        TS = journal.TS;
        TS0 = journal.TS0;
        TS1 = journal.TS1;
        Status0 = journal.Status0;
        Status1 = journal.Status1;
        IDAlarm = journal.IDAlarm;
        Alarm = journal.Alarm?.ToDTO()!;
        Station = journal.Station;
        IsRead = journal.IsRead;
    }

    public override Journal ToModel()
    {
        return new Journal(this);
    }
}