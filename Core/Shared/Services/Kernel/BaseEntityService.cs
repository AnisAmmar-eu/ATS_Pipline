using System.Linq.Expressions;
using Core.Shared.Models.DB.Kernel.Interfaces;
using Core.Shared.Models.DTO.Kernel.Interfaces;
using Core.Shared.Paginations;
using Core.Shared.Repositories.Kernel.Interfaces;
using Core.Shared.Services.Kernel.Interfaces;
using Core.Shared.UnitOfWork.Interfaces;

namespace Core.Shared.Services.Kernel;

public class BaseEntityService<TRepository, T, TDTO> : IBaseEntityService<T, TDTO>
	where TRepository : IBaseEntityRepository<T, TDTO>
	where T : class, IBaseEntity<T, TDTO>
	where TDTO : class, IDTO<T, TDTO>
{
	private readonly TRepository _repository;
	protected readonly IAnodeUOW _anodeUOW;

	protected BaseEntityService(IAnodeUOW anodeUOW)
	{
		_anodeUOW = anodeUOW;
		_repository = (TRepository?)anodeUOW.GetRepoByType(typeof(TRepository)) ??
			throw new InvalidOperationException("Repo is null");
	}

	public async Task<TDTO> GetByID(
		int id,
		Expression<Func<T, bool>>[]? filters = null,
		bool withTracking = true,
		params string[] includes) => (await _repository.GetById(id, filters, withTracking, includes)).ToDTO();

	public async Task<List<TDTO>> GetAll(
		Expression<Func<T, bool>>[]? filters = null,
		Func<IQueryable<T>, IOrderedQueryable<T>>? orderBy = null,
		bool withTracking = true,
		int? maxCount = null,
		params string[] includes)
	{
		return (await _repository.GetAll(filters, orderBy, withTracking, maxCount, includes)).ConvertAll(entity =>
			entity.ToDTO());
	}

	public async Task<List<TDTO>> GetWithPagination(Pagination pagination, int nbItems)

	{
		return (await _repository.GetWithPagination(pagination, nbItems)).ConvertAll(entity =>
			entity.ToDTO());
	}

	public Task<int> CountWithPagination(Pagination pagination) => _repository.CountWithPagination(pagination);

	public async Task<TDTO> Add(T entity)
	{
		await _anodeUOW.StartTransaction();
		await _repository.Add(entity);
		_anodeUOW.Commit();
		await _anodeUOW.CommitTransaction();
		return entity.ToDTO();
	}

	public async Task<List<TDTO>> AddAll(IEnumerable<T> entities)
	{
		await _anodeUOW.StartTransaction();
		List<TDTO> result = [];
		foreach (T entity in entities)
		{
			await _repository.Add(entity);
			result.Add(entity.ToDTO());
		}

		_anodeUOW.Commit();
		await _anodeUOW.CommitTransaction();
		return result;
	}

	public async Task<TDTO> Update(T entity)
	{
		await _anodeUOW.StartTransaction();
		_repository.Update(entity);
		_anodeUOW.Commit();
		await _anodeUOW.CommitTransaction();
		return entity.ToDTO();
	}

	public async Task<List<TDTO>> UpdateAll(IEnumerable<T> entities)
	{
		await _anodeUOW.StartTransaction();
		List<TDTO> result = [];
		foreach (T entity in entities)
		{
			_repository.Update(entity);
			result.Add(entity.ToDTO());
		}

		_anodeUOW.Commit();
		await _anodeUOW.CommitTransaction();
		return result;
	}

	public async Task Remove(T entity)
	{
		await _anodeUOW.StartTransaction();
		_repository.Remove(entity);
		_anodeUOW.Commit();
		await _anodeUOW.CommitTransaction();
	}

	public async Task Remove(int id, params string[] includes)
	{
		await _anodeUOW.StartTransaction();
		await _repository.Remove(id, includes);
		_anodeUOW.Commit();
		await _anodeUOW.CommitTransaction();
	}

	public async Task RemoveByLifeSpan(TimeSpan lifeSpan) => await _repository.RemoveByLifeSpan(lifeSpan);

	public async Task RemoveAll(IEnumerable<T> entities)
	{
		await _anodeUOW.StartTransaction();
		foreach (T entity in entities)
			_repository.Remove(entity);

		_anodeUOW.Commit();
		await _anodeUOW.CommitTransaction();
	}
}