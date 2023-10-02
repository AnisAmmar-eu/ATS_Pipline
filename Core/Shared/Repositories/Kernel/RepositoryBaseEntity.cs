using System.Linq.Expressions;
using Core.Shared.Exceptions;
using Core.Shared.Models.DB.Kernel.Interfaces;
using Core.Shared.Models.DTOs.Kernel.Interfaces;
using Core.Shared.Repositories.Kernel.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Core.Shared.Repositories.Kernel;

/// <summary>
///     Implements the <see cref="IRepositoryBase{T}" /> interface
/// </summary>
/// <typeparam name="TContext"> <see cref="DbContext" /> of the project</typeparam>
/// <typeparam name="T">
///     Type that defines an table in the database and have to implement <see cref="IBaseEntity{T}" />
/// </typeparam>
public class RepositoryBaseEntity<TContext, T, TDTO> : IRepositoryBaseEntity<T, TDTO>
	where T : class, IBaseEntity<T, TDTO>
	where TDTO : class, IDTO<T, TDTO>
	where TContext : DbContext
{
	protected TContext _context;
	protected ICollection<Expression<Func<T, bool>>> _importFilters = new List<Expression<Func<T, bool>>>();

	/// <summary>
	///     Constructor
	/// </summary>
	/// <param name="context"><see cref="DbContext" /> of the project</param>
	public RepositoryBaseEntity(TContext context)
	{
		_context = context;
	}

	/// <summary>
	///     Get an entity based on ID from the table of <typeref name="T" /> with join to its navigation properties
	/// </summary>
	/// <param name="id"></param>
	/// <param name="includes">Variadic parameter, array of <see cref="string" /> of names of column to include in the query</param>
	/// <returns>The entity <see cref="T" /></returns>
	public async Task<T> GetById(
		int id,
		Expression<Func<T, bool>>[]? filters = null,
		bool withTracking = true,
		params string[] includes
	)
	{
		T? t = await Query(
			filters,
			null,
			withTracking,
			includes: new Dictionary<string, string[]> { { "", includes } }
		).FirstOrDefaultAsync(x => x.ID == id);
		if (t == null)
			throw new EntityNotFoundException(typeof(T).Name, id);

		return t;
	}

	public async Task<T> GetByIdWithConcat(
		int id,
		Expression<Func<T, bool>>[]? filters = null,
		bool withTracking = true,
		Dictionary<string, string[]>? includes = null
	)
	{
		T? t = await Query(
			filters,
			null,
			withTracking,
			includes: includes
		).FirstOrDefaultAsync(x => x.ID == id);
		if (t == null)
			throw new EntityNotFoundException(typeof(T).Name, id);

		return t;
	}

	/// <summary>
	///     Get an entity from the table of <typeref name="T" /> with join to its navigation properties
	/// </summary>
	/// <param name="filters"></param>
	/// <param name="orderBy"></param>
	/// <param name="withTracking"></param>
	/// <param name="includes"></param>
	/// <returns>The entity <see cref="T" /></returns>
	public async Task<T> GetBy(
		Expression<Func<T, bool>>[]? filters = null,
		Func<IQueryable<T>, IOrderedQueryable<T>>? orderBy = null,
		bool withTracking = true,
		params string[] includes
	)
	{
		T? t = await Query(filters, null, withTracking).FirstOrDefaultAsync();
		if (t == null)
			throw new EntityNotFoundException((typeof(T).Name) + " not found");

		return t;
	}

	public async Task<T> GetByWithConcat(
		Expression<Func<T, bool>>[]? filters = null,
		Func<IQueryable<T>, IOrderedQueryable<T>>? orderBy = null,
		bool withTracking = true,
		Dictionary<string, string[]>? includes = null
	)
	{
		T? t = await Query(filters, orderBy, withTracking, includes: includes).FirstOrDefaultAsync();
		if (t == null)
			throw new EntityNotFoundException((typeof(T).Name) + " not found");

		return t;
	}

	/// <summary>
	///     Get all entities from the table of <typeref name="T" /> with join to its navigation properties
	/// </summary>
	/// <param name="includes">Variadic parameter, array of <see cref="string" /> of names of column to include in the query</param>
	/// <returns></returns>
	public async Task<List<T>> GetAll(
		Expression<Func<T, bool>>[]? filters = null,
		Func<IQueryable<T>, IOrderedQueryable<T>>? orderBy = null,
		bool withTracking = true,
		int? maxCount = null,
		params string[] includes
	)
	{
		//return await Query(filters, orderBy, withTracking, maxCount).ToListAsync();
		return await Query(filters, orderBy, withTracking, maxCount,
			new Dictionary<string, string[]> { { "", includes } }).ToListAsync();
	}

	public async Task<List<T>> GetAllWithConcat(
		Expression<Func<T, bool>>[]? filters = null,
		Func<IQueryable<T>, IOrderedQueryable<T>>? orderBy = null,
		bool withTracking = true,
		int? maxCount = null,
		Dictionary<string, string[]>? includes = null
	)
	{
		return await Query(filters, orderBy, withTracking, maxCount, includes).ToListAsync();
	}

	/// <summary>
	///     Find entities by a predicate
	/// </summary>
	/// <param name="expression">Predicate</param>
	/// <returns></returns>
	public async Task<List<T>> Find(Expression<Func<T, bool>> expression)
	{
		return await _context.Set<T>().Where(expression).ToListAsync();
	}

	/// <summary>
	///     Add an new entity in the table of <typeref name="T" />
	/// </summary>
	/// <param name="dto"><see cref="TDTO{T}" /> dto to use to instantiate the new entity</param>
	/// <returns></returns>
	public async Task Add(T entity)
	{
		await _context.Set<T>().AddAsync(entity);
	}

	/// <summary>
	///     Add an new entity in the table of <typeref name="T" /> and return the new entity as <see cref="IDTO" />
	/// </summary>
	/// <param name="dto"></param>
	/// <returns>The entity <see cref="T" /> saved in the database</returns>
	public async Task<T> AddAndReturn(T entity)
	{
		await _context.Set<T>().AddAsync(entity);
		return entity;
	}

	/// <summary>
	///     Add several entities in the table of <typeref name="T" />
	/// </summary>
	/// <param name="entities"><see cref="IEnumerable{T}" /> of entity to instantiate in the db</param>
	/// <returns></returns>
	public async Task AddRange(IEnumerable<T> entities)
	{
		await _context.Set<T>().AddRangeAsync(entities);
	}

	/// <summary>
	///     Remove an entity in the table of <typeref name="T" />
	/// </summary>
	/// <param name="entity">The entity <see cref="T" /> to remove</param>
	/// <returns></returns>
	public void Remove(T entity)
	{
		_context.Set<T>().Remove(entity);
	}

	/// <summary>
	///     Remove an entitiy in the table of <typeref name="T" /> with the given ID
	/// </summary>
	/// <param name="id">ID of the entity to remove</param>
	/// <param name="includes"></param>
	/// <returns></returns>
	public async Task Remove(int id, params string[] includes)
	{
		IQueryable<T> query = _context.Set<T>().AsQueryable();
		query = includes.Aggregate(query, (current, include) => current.Include(include));
		T? entity = await query.FirstOrDefaultAsync(x => x.ID == id);
		if (entity == null)
			throw new EntityNotFoundException(typeof(T).Name, id);

		_context.Set<T>().Remove(entity);
	}

	/// <summary>
	///     Remove several entities in the table of <typeref name="T" />
	/// </summary>
	/// <param name="entities"><see cref="IEnumerable{T}" /> of entity to remove</param>
	/// <returns></returns>
	public void RemoveRange(IEnumerable<T> entities)
	{
		_context.Set<T>().RemoveRange(entities);
	}

	/// <summary>
	///     Update an entity in the table of <typeref name="T" /> and return the updated entity
	/// </summary>
	/// <param name="entity">Entity to updated, null attribute will not change</param>
	/// <returns>The updated entity <see cref="T" /></returns>
	public T Update(T entity)
	{
		_context.Set<T>().Update(entity);
		return entity;
	}

	/// <summary>
	///     Check if an element exist with the predication
	/// </summary>
	/// <param name="predicate"></param>
	/// <param name="includes"></param>
	/// <returns></returns>
	public async Task<bool> Any(Expression<Func<T, bool>> predicate, bool withTracking = true, params string[] includes)
	{
		return await Query(
			new[] { predicate },
			null,
			withTracking,
			null,
			new Dictionary<string, string[]> { { "", includes } }
		).AnyAsync(predicate);
	}

	private IQueryable<T> Query(
		Expression<Func<T, bool>>[]? filters = null,
		Func<IQueryable<T>, IOrderedQueryable<T>>? orderBy = null,
		bool withTracking = true,
		int? maxCount = null,
		Dictionary<string, string[]>? includes = null
	)
	{
		IQueryable<T> query = _context.Set<T>().AsQueryable();
		if (includes != null)
		{
			foreach (KeyValuePair<string, string[]> include in includes)
				if (include.Key != "")
				{
					query = query.Include(include.Key);
					query = include.Value.Aggregate(query,
						(current, value) => current.Include(include.Key + "." + value));
				}
				else
				{
					query = include.Value.Aggregate(query, (current, value) => current.Include(value));
				}

			if (includes.Count > 0)
				// WARNING - https://learn.microsoft.com/fr-fr/ef/core/querying/single-split-queries
				query = query.AsSplitQuery();
		}

		if (!withTracking) query = query.AsNoTracking();

		if (_importFilters.Count > 0)
			query = _importFilters.Aggregate(query, (current, filter) => current.Where(filter));

		if (filters != null) query = filters.Aggregate(query, (current, filter) => current.Where(filter));

		if (maxCount != null) query = query.Take(maxCount.Value);

		if (orderBy != null)
			return orderBy(query);
		return query;
	}
}