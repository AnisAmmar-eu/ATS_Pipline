using Core.Shared.Data;
using Microsoft.AspNetCore.SignalR;

namespace Core.Shared.signalR
{
    public class alarmesHub : Hub
    {
        private readonly AlarmesDbContext _AlarmesDbContext;


        public alarmesHub(AlarmesDbContext alarmesDbContext)
        {
            _AlarmesDbContext = alarmesDbContext;      
        }


        public async Task RequestJournalData()
        {
            var journalData = _AlarmesDbContext.Journal.ToList();
            await Clients.Caller.SendAsync("ReceiveAlarm", journalData);
        }


    }
}
