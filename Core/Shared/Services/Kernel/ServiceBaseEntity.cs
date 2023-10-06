using System.Linq.Expressions;
using Core.Shared.Models.DB.Kernel.Interfaces;
using Core.Shared.Models.DTO.Kernel.Interfaces;
using Core.Shared.Repositories.Kernel.Interfaces;
using Core.Shared.Services.Kernel.Interfaces;
using Core.Shared.UnitOfWork.Interfaces;

namespace Core.Shared.Services.Kernel;

public class ServiceBaseEntity<TRepository, T, TDTO> : IServiceBaseEntity<T, TDTO>
	where T : class, IBaseEntity<T, TDTO>
	where TDTO : class, IDTO<T, TDTO>
	where TRepository : IRepositoryBaseEntity<T, TDTO>
{
	private readonly TRepository _repository;
	protected readonly IAlarmUOW AlarmUOW;

	public ServiceBaseEntity(IAlarmUOW alarmUOW)
	{
		AlarmUOW = alarmUOW;
		_repository = (TRepository?)alarmUOW.GetRepoByType(typeof(TRepository)) ??
		              throw new InvalidOperationException("Repo is null");
	}

	public async Task<TDTO> GetById(int id,
		Expression<Func<T, bool>>[]? filters = null,
		bool withTracking = true,
		params string[] includes)
	{
		return (await _repository.GetById(id, filters, withTracking, includes)).ToDTO();
	}

	public async Task<List<TDTO>> GetAll(Expression<Func<T, bool>>[]? filters = null,
		Func<IQueryable<T>, IOrderedQueryable<T>>? orderBy = null,
		bool withTracking = true,
		int? maxCount = null,
		params string[] includes)
	{
		return (await _repository.GetAll(filters, orderBy, withTracking, maxCount, includes)).ConvertAll(entity =>
			entity.ToDTO());
	}

	public async Task<TDTO> Add(T entity)
	{
		await AlarmUOW.StartTransaction();
		await _repository.Add(entity);
		AlarmUOW.Commit();
		await AlarmUOW.CommitTransaction();
		return entity.ToDTO();
	}

	public async Task<List<TDTO>> AddAll(IEnumerable<T> entities)
	{
		await AlarmUOW.StartTransaction();
		List<TDTO> result = new List<TDTO>();
		foreach (T entity in entities)
		{
			await _repository.Add(entity);
			result.Add(entity.ToDTO());
		}

		AlarmUOW.Commit();
		await AlarmUOW.CommitTransaction();
		return result;
	}

	public async Task<TDTO> Update(T entity)
	{
		await AlarmUOW.StartTransaction();
		_repository.Update(entity);
		AlarmUOW.Commit();
		await AlarmUOW.CommitTransaction();
		return entity.ToDTO();
	}

	public async Task<TDTO> Remove(T entity)
	{
		await AlarmUOW.StartTransaction();
		_repository.Remove(entity);
		AlarmUOW.Commit();
		await AlarmUOW.CommitTransaction();
		return entity.ToDTO();
	}
}