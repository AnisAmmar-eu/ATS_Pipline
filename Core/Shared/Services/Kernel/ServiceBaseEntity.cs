using System.Linq.Expressions;
using Core.Shared.Models.DB.Kernel.Interfaces;
using Core.Shared.Models.DTO.Kernel.Interfaces;
using Core.Shared.Paginations;
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
	protected readonly IAnodeUOW AnodeUOW;

	public ServiceBaseEntity(IAnodeUOW anodeUOW)
	{
		AnodeUOW = anodeUOW;
		_repository = (TRepository?)anodeUOW.GetRepoByType(typeof(TRepository)) ??
		              throw new InvalidOperationException("Repo is null");
	}

	public async Task<TDTO> GetByID(int id,
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

	public async Task<List<TDTO>> GetWithPagination(Pagination pagination, int nbItems, int lastID)

	{
		return (await _repository.GetWithPagination(pagination, nbItems, lastID)).ConvertAll(entity =>
			entity.ToDTO());
	}

	public async Task<TDTO> Add(T entity)
	{
		await AnodeUOW.StartTransaction();
		await _repository.Add(entity);
		AnodeUOW.Commit();
		await AnodeUOW.CommitTransaction();
		return entity.ToDTO();
	}

	public async Task<List<TDTO>> AddAll(IEnumerable<T> entities)
	{
		await AnodeUOW.StartTransaction();
		List<TDTO> result = new();
		foreach (T entity in entities)
		{
			await _repository.Add(entity);
			result.Add(entity.ToDTO());
		}

		AnodeUOW.Commit();
		await AnodeUOW.CommitTransaction();
		return result;
	}

	public async Task<TDTO> Update(T entity)
	{
		await AnodeUOW.StartTransaction();
		_repository.Update(entity);
		AnodeUOW.Commit();
		await AnodeUOW.CommitTransaction();
		return entity.ToDTO();
	}

	public async Task<List<TDTO>> UpdateAll(IEnumerable<T> entities)
	{
		await AnodeUOW.StartTransaction();
		List<TDTO> result = new();
		foreach (T entity in entities)
		{
			_repository.Update(entity);
			result.Add(entity.ToDTO());
		}

		AnodeUOW.Commit();
		await AnodeUOW.CommitTransaction();
		return result;
	}

	public async Task<TDTO> Remove(T entity)
	{
		await AnodeUOW.StartTransaction();
		_repository.Remove(entity);
		AnodeUOW.Commit();
		await AnodeUOW.CommitTransaction();
		return entity.ToDTO();
	}
    
	public async Task<List<TDTO>> RemoveAll(IEnumerable<T> entities)
	{
		await AnodeUOW.StartTransaction();
		List<TDTO> result = new();
		foreach (T entity in entities)
		{
			_repository.Remove(entity);
			result.Add(entity.ToDTO());
		}

		AnodeUOW.Commit();
		await AnodeUOW.CommitTransaction();
		return result;
	}
}