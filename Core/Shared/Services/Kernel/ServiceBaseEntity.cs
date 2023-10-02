using System.Linq.Expressions;
using System.Reflection;
using Core.Shared.Data;
using Core.Shared.Models.DB.Kernel.Interfaces;
using Core.Shared.Models.DTOs.Kernel.Interfaces;
using Core.Shared.Repositories.Kernel;
using Core.Shared.Repositories.Kernel.Interfaces;
using Core.Shared.Services.Kernel.Interfaces;
using Core.Shared.UnitOfWork;
using Core.Shared.UnitOfWork.Interfaces;
using Microsoft.VisualBasic;

namespace Core.Shared.Services.Kernel;

public class ServiceBaseEntity<TRepository, T, TDTO> : IServiceBaseEntity<T, TDTO>
	where T : class, IBaseEntity<T, TDTO>
	where TDTO : class, IDTO<T, TDTO>
	where TRepository : IRepositoryBaseEntity<T, TDTO>
{
	protected readonly IAlarmUOW _alarmUOW;
	private readonly TRepository _repository;

	protected ServiceBaseEntity(IAlarmUOW alarmUOW)
	{
		_alarmUOW = alarmUOW;
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
		await _alarmUOW.StartTransaction();
		await _repository.Add(entity);
		_alarmUOW.Commit();
		await _alarmUOW.CommitTransaction();
		return entity.ToDTO();
	}

	public async Task<TDTO> Update(T entity)
	{
		await _alarmUOW.StartTransaction();
		_repository.Update(entity);
		_alarmUOW.Commit();
		await _alarmUOW.CommitTransaction();
		return entity.ToDTO();
	}

	public async Task<TDTO> Remove(T entity)
	{
		await _alarmUOW.StartTransaction();
		_repository.Remove(entity);
		_alarmUOW.Commit();
		await _alarmUOW.CommitTransaction();
		return entity.ToDTO();
	}
}