using System.Linq.Expressions;
using Core.Entities.KPI.KPICs.Models.DB;
using Core.Entities.KPI.KPICs.Services;
using Core.Entities.KPI.KPIEntries.Dictionaries;
using Core.Entities.KPI.KPIEntries.Models.DB.KPILogs;
using Core.Entities.KPI.KPIEntries.Models.DB.KPIRTs;
using Core.Entities.KPI.KPIEntries.Models.DTO.KPILogs;
using Core.Entities.KPI.KPIEntries.Models.DTO.KPIRTs;
using Core.Entities.KPI.KPIEntries.Repositories.KPIRTs;
using Core.Entities.KPI.KPIEntries.Services.KPILogs;
using Core.Shared.Models.DB.Kernel.Interfaces;
using Core.Shared.Models.DTO.Kernel.Interfaces;
using Core.Shared.Services.Kernel;
using Core.Shared.Services.Kernel.Interfaces;
using Core.Shared.UnitOfWork.Interfaces;

namespace Core.Entities.KPI.KPIEntries.Services.KPIRTs;

public class KPIRTService : ServiceBaseEntity<IKPIRTRepository, KPIRT, DTOKPIRT>, IKPIRTService
{
	private IKPILogService _kpiLogService;

	public KPIRTService(IAlarmUOW alarmUOW, IKPILogService kpiLogService) : base(alarmUOW)
	{
		_kpiLogService = kpiLogService;
	}

	public async Task<List<DTOKPILog>> SaveRTsToLogs(List<string> periodsToSave)
	{
		List<KPIRT> kpiRTs = await AlarmUOW.KPIRT.GetAll(new Expression<Func<KPIRT, bool>>[]
		{
			kpiRT => periodsToSave.Contains(kpiRT.Period)
		}, null, false, null);
		List<KPILog> kpiLogs = new();
		foreach (KPIRT kpiRT in kpiRTs)
			kpiLogs.Add(kpiRT.ToLog(await AlarmUOW.KPIC.GetById(kpiRT.KPICID)));

		return await _kpiLogService.AddAll(kpiLogs);
	}

	#region KPIRT computing

	// See summary in interface
	public async Task ComputeKPIRTs<T, TDTO, TService, TValue>(TService tService)
		where T : class, IBaseEntity<T, TDTO>
		where TDTO : class, IDTO<T, TDTO>, IBaseKPI<TValue>
		where TService : class, IServiceBaseEntity<T, TDTO>
	{
		// Logic -> GetAll of T, get its KPICRIDs (create KPIRT if necessary) and make the computations on it.
		List<TDTO> tDTOs = await tService.GetAll();
		if (tDTOs.Count == 0)
			return;
		string[] kpiCRIDs = tDTOs[0].GetKPICRID();
		string[] periods = { KPIPeriod.Day, KPIPeriod.Week, KPIPeriod.Month, KPIPeriod.Year };

		List<List<KPIRT>> kpiRTs = await GetKPIRTsFromKPICsAndPeriods(kpiCRIDs, periods);

		List<List<TValue>> tValuesPerPeriods = GetValuesFromPeriods<T, TDTO, TValue>(tDTOs, periods);

		await AlarmUOW.StartTransaction();
		Func<List<TValue>, string>[] functions = tDTOs[0].GetComputedValue();
		for (int i = 0; i < kpiRTs.Count; ++i)
		{
			List<KPIRT> kpiRTPerC = kpiRTs[i];
			for (int j = 0; j < kpiRTPerC.Count; ++j)
			{
				kpiRTPerC[j].Value = functions[i].Invoke(tValuesPerPeriods[j]);
				AlarmUOW.KPIRT.Update(kpiRTPerC[j]);
			}
		}

		AlarmUOW.Commit();
		await AlarmUOW.CommitTransaction();
	}

	/// <summary>
	///		Split the KPIRTs depending on kpiCRIDs. It is then subdivided into periods.
	///		eg. If there is 2 kpiCRIDS, there would be 2 lists of 4 KPIRTs each.
	///		Every member of each list has the same KPICID than other KPIRTs in its list.
	///		Then, the index in the sublists determine to which period it is associated.
	///		If there is no KPIRT associated with given KPICRID & period, a new one will be created.
	/// </summary>
	/// <param name="kpiCRIDs">
	///		List of kpiCRIDs used to divide KPIRTs into period lists.
	/// </param>
	/// <param name="periods">
	///		List of periods used to subdivide KPIRTs depending on which period they cover.
	/// </param>
	/// <returns>
	///		A list of lists of KPIRTs. First level divides by KPICID, second level by period.
	/// </returns>
	private async Task<List<List<KPIRT>>> GetKPIRTsFromKPICsAndPeriods(string[] kpiCRIDs, string[] periods)
	{
		List<List<KPIRT>> kpiRTs = new();
		await AlarmUOW.StartTransaction();
		foreach (string kpiCRID in kpiCRIDs)
		{
			List<KPIRT> localKPIRTs = new(periods.Length);
			List<KPIRT> availableKPIRTs = await AlarmUOW.KPIRT.GetAll(
				filters: new Expression<Func<KPIRT, bool>>[]
				{
					kpiRT => kpiRT.KPIC.RID == kpiCRID
				}, includes: "KPIC");
			for (int i = 0; i < localKPIRTs.Count; ++i)
			{
				KPIRT? kpiRT = availableKPIRTs.FirstOrDefault(kpiRT => kpiRT.Period == periods[i]);
				if (kpiRT == null)
				{
					KPIC kpiC = await AlarmUOW.KPIC.GetBy(filters: new Expression<Func<KPIC, bool>>[]
					{
						kpiC => kpiC.RID == kpiCRID
					});
					kpiRT = new KPIRT
					{
						KPICID = kpiC.ID,
						StationID = 0, // TODO
						Value = "",
						Period = periods[i],
						KPIC = kpiC,
					};
					await AlarmUOW.KPIRT.Add(kpiRT);
					AlarmUOW.Commit();
				}

				localKPIRTs[i] = kpiRT;
			}

			kpiRTs.Add(localKPIRTs);
		}

		await AlarmUOW.CommitTransaction();
		return kpiRTs;
	}

	/// <summary>
	///		Will get every value from tDTOs split into time periods from smallest time period to largest.
	/// </summary>
	/// <param name="tDTOs">List of DTOS of type tDTO</param>
	/// <param name="periods">List of periods used to determine time periods</param>
	/// <typeparam name="T">Entity type</typeparam>
	/// <typeparam name="TDTO">DTO type</typeparam>
	/// <typeparam name="TValue">Type of the value used in T to send to KPI</typeparam>
	/// <returns>
	///		A list of list of TValues, first level split those lists depending on time frames,
	///		second level is a collection of all values of all tDTOs in corresponding time frame.
	/// </returns>
	private List<List<TValue>> GetValuesFromPeriods<T, TDTO, TValue>(List<TDTO> tDTOs, string[] periods)
		where T : class, IBaseEntity<T, TDTO>
		where TDTO : class, IDTO<T, TDTO>, IBaseKPI<TValue>
	{
		// TODO Optimise in one list iteration
		DateTimeOffset now = DateTimeOffset.Now;
		List<List<TDTO>> tDTOsPerPeriod = new(periods.Length + 1) { [periods.Length] = tDTOs };
		List<List<TValue>> tValuesPerPeriods = new(periods.Length);
		for (int i = periods.Length - 1; i <= 0; --i)
		{
			DateTimeOffset startRange = KPIPeriod.GetStartRange(periods[i], now);
			List<TDTO> tDTOPeriod = new();
			List<TValue> tValuePeriod = new();
			foreach (TDTO tDTO in tDTOsPerPeriod[i + 1])
			{
				if (tDTO.TS < startRange)
					continue;
				tDTOPeriod.Add(tDTO);
				tValuePeriod.Add(tDTO.GetValue());
			}

			tDTOsPerPeriod.Add(tDTOPeriod);
			tValuesPerPeriods.Add(tValuePeriod);
		}

		return tValuesPerPeriods;
	}

	#endregion
}