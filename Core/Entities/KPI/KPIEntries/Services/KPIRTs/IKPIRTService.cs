using Core.Entities.KPI.KPIEntries.Models.DB.KPIRTs;
using Core.Entities.KPI.KPIEntries.Models.DTO.KPILogs;
using Core.Entities.KPI.KPIEntries.Models.DTO.KPIRTs;
using Core.Shared.Models.DB.Kernel.Interfaces;
using Core.Shared.Models.DTO.Kernel.Interfaces;
using Core.Shared.Services.Kernel.Interfaces;

namespace Core.Entities.KPI.KPIEntries.Services.KPIRTs;

public interface IKPIRTService : IServiceBaseEntity<KPIRT, DTOKPIRT>
{
	public Task<List<DTOKPILog>> SaveRTsToLogs(List<string> periodsToSave);

	/// <summary>
	///     This function will compute KPIRTs values for every period and every KPICRID for T class as a T class
	///     may have multiple values worth to be monitored.
	/// </summary>
	/// <param name="tService">Service of type T</param>
	/// <typeparam name="T">Monitored entity type</typeparam>
	/// <typeparam name="TDTO">DTO Type</typeparam>
	/// <typeparam name="TService">Type of T service</typeparam>
	/// <typeparam name="TValue">Type which is monitored in T</typeparam>
	public Task ComputeKPIRTs<T, TDTO, TService, TValue>(TService tService)
		where T : class, IBaseEntity<T, TDTO>
		where TDTO : class, IDTO<T, TDTO>, IBaseKPI<TValue>
		where TService : class, IServiceBaseEntity<T, TDTO>;
}