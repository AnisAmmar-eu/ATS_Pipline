using Core.Entities.KPI.KPIEntries.Models.DB.KPIRTs;
using Core.Entities.KPI.KPIEntries.Models.DTO.KPILogs;
using Core.Entities.KPI.KPIEntries.Models.DTO.KPIRTs;
using Core.Shared.Models.DB.Kernel.Interfaces;
using Core.Shared.Models.DTO.Kernel.Interfaces;
using Core.Shared.Repositories.Kernel.Interfaces;
using Core.Shared.Services.Kernel.Interfaces;

namespace Core.Entities.KPI.KPIEntries.Services.KPIRTs;

public interface IKPIRTService : IBaseEntityService<KPIRT, DTOKPIRT>
{
	public Task<List<DTOKPIRT>> GetByRIDsAndPeriod(string period, List<string> rids);

	/// <summary>
	/// Converts every KPIRT to its log version.
	/// </summary>
	/// <param name="periodsToSave"></param>
	/// <returns></returns>
	public Task<List<DTOKPILog>> SaveRTsToLogs(List<string> periodsToSave);

	/// <summary>
	///     This function will compute KPIRTs values for every period and every KPICRID for T class as a T class
	///     may have multiple values worth to be monitored.
	/// </summary>
	/// <param name="tRepository">Service of type T</param>
	/// <typeparam name="T">Monitored entity type</typeparam>
	/// <typeparam name="TDTO">DTO Type</typeparam>
	/// <typeparam name="TRepository">Type of T repository</typeparam>
	/// <typeparam name="TValue">Type which is monitored in T</typeparam>
	public Task ComputeKPIRTs<T, TDTO, TRepository, TValue>(TRepository tRepository)
		where T : class, IBaseEntity<T, TDTO>, IBaseKPI<TValue>
		where TDTO : class, IDTO<T, TDTO>
		where TRepository : class, IBaseEntityRepository<T, TDTO>;
}