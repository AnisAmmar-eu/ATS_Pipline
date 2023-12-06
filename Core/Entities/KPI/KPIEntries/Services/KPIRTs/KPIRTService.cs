using Core.Entities.KPI.KPICs.Models.DB;
using Core.Entities.KPI.KPIEntries.Dictionaries;
using Core.Entities.KPI.KPIEntries.Models.DB.KPILogs;
using Core.Entities.KPI.KPIEntries.Models.DB.KPIRTs;
using Core.Entities.KPI.KPIEntries.Models.DTO.KPILogs;
using Core.Entities.KPI.KPIEntries.Models.DTO.KPIRTs;
using Core.Entities.KPI.KPIEntries.Repositories.KPIRTs;
using Core.Entities.KPI.KPIEntries.Services.KPILogs;
using Core.Shared.Models.DB.Kernel.Interfaces;
using Core.Shared.Models.DTO.Kernel.Interfaces;
using Core.Shared.Repositories.Kernel.Interfaces;
using Core.Shared.Services.Kernel;
using Core.Shared.UnitOfWork.Interfaces;

namespace Core.Entities.KPI.KPIEntries.Services.KPIRTs;

public class KPIRTService : BaseEntityService<IKPIRTRepository, KPIRT, DTOKPIRT>, IKPIRTService
{
	private readonly IKPILogService _kpiLogService;

	public KPIRTService(IAnodeUOW anodeUOW, IKPILogService kpiLogService) : base(anodeUOW)
	{
		_kpiLogService = kpiLogService;
	}

	public async Task<List<DTOKPIRT>> GetByRIDsAndPeriod(string period, List<string> rids)
	{
		period = period.ToUpper();
		return (await AnodeUOW.KPIRT.GetAll(
			[kpiRT => kpiRT.Period == period && rids.Contains(kpiRT.KPIC.RID)],
			withTracking: false,
			includes: nameof(KPIRT.KPIC)))
			.ConvertAll(kpiRT => kpiRT.ToDTO());
	}

	public async Task<List<DTOKPILog>> SaveRTsToLogs(List<string> periodsToSave)
	{
		List<KPIRT> kpiRTs = await AnodeUOW.KPIRT.GetAll([kpiRT => periodsToSave.Contains(kpiRT.Period)], null, false);
		List<KPILog> kpiLogs = [];
		foreach (KPIRT kpiRT in kpiRTs)
			kpiLogs.Add(kpiRT.ToLog(await AnodeUOW.KPIC.GetById(kpiRT.KPICID)));

		return await _kpiLogService.AddAll(kpiLogs);
	}

	#region KPIRT computing

	/// <summary>
	/// See summary in interface
	/// </summary>
	/// <typeparam name="T"></typeparam>
	/// <typeparam name="TDTO"></typeparam>
	/// <typeparam name="TRepository"></typeparam>
	/// <typeparam name="TValue"></typeparam>
	/// <param name="tRepository"></param>
	public async Task ComputeKPIRTs<T, TDTO, TRepository, TValue>(TRepository tRepository)
		where T : class, IBaseEntity<T, TDTO>, IBaseKPI<TValue>
		where TDTO : class, IDTO<T, TDTO>
		where TRepository : class, IBaseEntityRepository<T, TDTO>
	{
		string[] periods = { KPIPeriod.Day, KPIPeriod.Week, KPIPeriod.Month, KPIPeriod.Year };
		DateTimeOffset oldest = KPIPeriod.GetStartRange(KPIPeriod.Year, DateTimeOffset.Now);
		// Logic -> GetAll of T, get its KPICRIDs (create KPIRT if necessary) and make the computations on it.
		List<T> entities = await tRepository.GetAll([entity => entity.TS >= oldest], withTracking: false);
		if (entities.Count == 0)
			return;

		string[] kpiCRIDs = T.GetKPICRID();

		List<List<KPIRT>> kpiRTs = await GetKPIRTsFromKPICsAndPeriods(kpiCRIDs, periods);

		List<List<TValue>> tValuesPerPeriods = GetValuesFromPeriods<T, TDTO, TValue>(entities, periods);

		await AnodeUOW.StartTransaction();
		// We compute all values for every KPIC at once to be faster and then we do it for every time period.
		Func<List<TValue>, string[]> function = entities[0].GetComputedValues();
		for (int i = 0; i < periods.Length; ++i)
		{
			string[] values = function.Invoke(tValuesPerPeriods[i]);
			for (int j = 0; j < kpiRTs.Count; ++j)
			{
				kpiRTs[j][i].Value = values[j];
				AnodeUOW.KPIRT.Update(kpiRTs[j][i]);
			}
		}

		AnodeUOW.Commit();
		await AnodeUOW.CommitTransaction();
	}

	/// <summary>
	///     Split the KPIRTs depending on kpiCRIDs. It is then subdivided into periods.
	///     eg. If there is 2 kpiCRIDS, there would be 2 lists of 4 KPIRTs each.
	///     Every member of each list has the same KPICID than other KPIRTs in its list.
	///     Then, the index in the sublists determine to which period it is associated.
	///     If there is no KPIRT associated with given KPICRID & period, a new one will be created.
	/// </summary>
	/// <param name="kpiCRIDs">
	///     List of kpiCRIDs used to divide KPIRTs into period lists.
	/// </param>
	/// <param name="periods">
	///     List of periods used to subdivide KPIRTs depending on which period they cover.
	/// </param>
	/// <returns>
	///     A list of lists of KPIRTs. First level divides by KPICID, second level by period.
	/// </returns>
	private async Task<List<List<KPIRT>>> GetKPIRTsFromKPICsAndPeriods(string[] kpiCRIDs, string[] periods)
	{
		List<List<KPIRT>> kpiRTs = new();
		await AnodeUOW.StartTransaction();
		foreach (string kpiCRID in kpiCRIDs)
		{
			KPIC kpiC = await AnodeUOW.KPIC.GetBy([kpiC => kpiC.RID == kpiCRID], withTracking: true);
			List<KPIRT> localKPIRTs = new(periods.Length);
			List<KPIRT> availableKPIRTs = await AnodeUOW.KPIRT.GetAll([kpiRT => kpiRT.KPICID == kpiC.ID], withTracking: false);
			foreach (string period in periods)
			{
				KPIRT? kpiRT = availableKPIRTs.Find(kpiRT => kpiRT.Period == period);
				if (kpiRT is null)
				{
					kpiRT = new() {
						KPICID = kpiC.ID,
						Value = string.Empty,
						Period = period,
						KPIC = kpiC,
					};
					await AnodeUOW.KPIRT.Add(kpiRT);
				}
				else
				{
					kpiRT.KPIC = kpiC;
				}

				localKPIRTs.Add(kpiRT);
			}

			kpiRTs.Add(localKPIRTs);
		}

		AnodeUOW.Commit();
		await AnodeUOW.CommitTransaction();
		return kpiRTs;
	}

	/// <summary>
	///     Will get every value from tDTOs split into time periods from smallest time period to largest.
	/// </summary>
	/// <param name="entities">List of DTOS of type tDTO</param>
	/// <param name="periods">List of periods used to determine time periods</param>
	/// <typeparam name="T">Entity type</typeparam>
	/// <typeparam name="TDTO">DTO type</typeparam>
	/// <typeparam name="TValue">Type of the value used in T to send to KPI</typeparam>
	/// <returns>
	///     A list of list of TValues, first level split those lists depending on time frames,
	///     second level is a collection of all values of all tDTOs in corresponding time frame.
	/// </returns>
	private static List<List<TValue>> GetValuesFromPeriods<T, TDTO, TValue>(List<T> entities, string[] periods)
		where T : class, IBaseEntity<T, TDTO>, IBaseKPI<TValue>
		where TDTO : class, IDTO<T, TDTO>
	{
		DateTimeOffset now = DateTimeOffset.Now;
		List<List<TValue>> tValuesPerPeriods = new();
		List<DateTimeOffset> periodsStartRange = new();
		foreach (string period in periods)
		{
			tValuesPerPeriods.Add(new List<TValue>());
			periodsStartRange.Add(KPIPeriod.GetStartRange(period, now));
		}

		foreach (T entity in entities)
		{
			for (int i = 0; i < periods.Length; ++i)
			{
				if (entity.TS >= periodsStartRange[i])
					tValuesPerPeriods[i].Add(entity.GetValue());
			}
		}

		return tValuesPerPeriods;
	}

	#endregion
}