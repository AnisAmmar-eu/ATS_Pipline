using System.Linq.Expressions;
using Core.Shared.Models.DB.Kernel.Interfaces;
using Core.Shared.Models.DTO.Kernel.Interfaces;

namespace Core.Shared.Services.Kernel.Interfaces;

public interface IServiceBaseEntity<T, TDTO>
	where T : class, IBaseEntity<T, TDTO>
	where TDTO : class, IDTO<T, TDTO>
{
	public Task<TDTO> GetByID(int id,
		Expression<Func<T, bool>>[]? filters = null,
		bool withTracking = true,
		params string[] includes);

	public Task<List<TDTO>> GetAll(Expression<Func<T, bool>>[]? filters = null,
		Func<IQueryable<T>, IOrderedQueryable<T>>? orderBy = null,
		bool withTracking = true,
		int? maxCount = null,
		params string[] includes);

	public Task<TDTO> Add(T entity);
	public Task<List<TDTO>> AddAll(IEnumerable<T> entities);
	public Task<TDTO> Update(T entity);
	public Task<TDTO> Remove(T entity);
}