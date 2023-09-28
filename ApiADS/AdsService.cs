using Core.Shared.Data;
using Core.Entities.Alarms.AlarmsPLC.Models.DB;

namespace testCVB
{
    public class AdsService
    {
        private readonly AlarmCTX _alarmCTX;

        public AdsService(AlarmCTX alarmCTX)
        {
            _alarmCTX = alarmCTX;
        }

        public async Task<bool> InsertAlarmPLC(AlarmPLC alarmPLC)
        {
            try
            {
                await _alarmCTX.AlarmPLC.AddAsync(alarmPLC);
                await _alarmCTX.SaveChangesAsync();
                return true;
            }
            catch
            {
                return false;
            }
        }

    }
}
