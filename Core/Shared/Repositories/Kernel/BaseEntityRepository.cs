using System.Linq.Expressions;
using Core.Shared.Exceptions;
using Core.Shared.Models.DB.Kernel.Interfaces;
using Core.Shared.Models.DTO.Kernel.Interfaces;
using Core.Shared.Paginations;
using Core.Shared.Paginations.Filtering;
using Core.Shared.Paginations.Sorting;
using Core.Shared.Repositories.Kernel.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Core.Shared.Repositories.Kernel;

/// <summary>
///     Implements the <see cref="IBaseEntityRepository{T,TDTO}" /> interface
/// </summary>
/// <typeparam name="TContext"> <see cref="DbContext" /> of the project</typeparam>
/// <typeparam name="T">
///     Type that defines an table in the database and have to implement <see cref="IBaseEntity{T}" />
/// </typeparam>
public class BaseEntityRepository<TContext, T, TDTO> : IBaseEntityRepository<T, TDTO>
	where T : class, IBaseEntity<T, TDTO>
	where TDTO : class, IDTO<T, TDTO>
	where TContext : DbContext
{
	private readonly ICollection<Expression<Func<T, bool>>> _importFilters = new List<Expression<Func<T, bool>>>();
	protected readonly TContext Context;

	/// <summary>
	///     Constructor
	/// </summary>
	/// <param name="context"><see cref="DbContext" /> of the project</param>
	public BaseEntityRepository(TContext context)
	{
		Context = context;
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
		T? t = await Query(filters, null, withTracking,
			includes: new Dictionary<string, string[]> { { "", includes } }).FirstOrDefaultAsync();
		if (t == null)
			throw new EntityNotFoundException(typeof(T).Name + " not found");

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
			throw new EntityNotFoundException(typeof(T).Name + " not found");

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

	public async Task<List<T>> GetWithPagination(Pagination pagination, int nbItems, int lastID)
	{
		// No split query because we are using .Take();
		// No tracking as this is used for back to front purposes and thus useless.
		// First line is aggregating every include
		IOrderedQueryable<T> query = pagination.Includes
			.Aggregate(Context.Set<T>()
				.AsQueryable(), (current, value) => current.Include(value))
			.AsNoTracking()
			.FilterFromPagination<T, TDTO>(pagination, lastID).SortFromPagination<T, TDTO>(pagination);
		if (nbItems == 0)
			return await query.ToListAsync();
		return await query.Take(nbItems).ToListAsync();
	}

	/// <summary>
	///     Find entities by a predicate
	/// </summary>
	/// <param name="expression">Predicate</param>
	/// <returns></returns>
	public async Task<List<T>> Find(Expression<Func<T, bool>> expression)
	{
		return await Context.Set<T>().Where(expression).ToListAsync();
	}

	/// <summary>
	///     Add an new entity in the table of <typeref name="T" />
	/// </summary>
	/// <param name="dto"><see cref="TDTO{T}" /> dto to use to instantiate the new entity</param>
	/// <returns></returns>
	public async Task Add(T entity)
	{
		await Context.Set<T>().AddAsync(entity);
	}

	/// <summary>
	///     Add an new entity in the table of <typeref name="T" /> and return the new entity as <see cref="IDTO" />
	/// </summary>
	/// <param name="dto"></param>
	/// <returns>The entity <see cref="T" /> saved in the database</returns>
	public async Task<T> AddAndReturn(T entity)
	{
		await Context.Set<T>().AddAsync(entity);
		return entity;
	}

	/// <summary>
	///     Add several entities in the table of <typeref name="T" />
	/// </summary>
	/// <param name="entities"><see cref="IEnumerable{T}" /> of entity to instantiate in the db</param>
	/// <returns></returns>
	public async Task AddRange(IEnumerable<T> entities)
	{
		await Context.Set<T>().AddRangeAsync(entities);
	}

	/// <summary>
	///     Remove an entity in the table of <typeref name="T" />
	/// </summary>
	/// <param name="entity">The entity <see cref="T" /> to remove</param>
	/// <returns></returns>
	public void Remove(T entity)
	{
		Context.Set<T>().Remove(entity);
	}

	/// <summary>
	///     Remove an entitiy in the table of <typeref name="T" /> with the given ID
	/// </summary>
	/// <param name="id">ID of the entity to remove</param>
	/// <param name="includes"></param>
	/// <returns></returns>
	public async Task Remove(int id, params string[] includes)
	{
		IQueryable<T> query = Context.Set<T>().AsQueryable();
		query = includes.Aggregate(query, (current, include) => current.Include(include));
		T? entity = await query.FirstOrDefaultAsync(x => x.ID == id);
		if (entity == null)
			throw new EntityNotFoundException(typeof(T).Name, id);

		Context.Set<T>().Remove(entity);
	}

	/// <summary>
	///     Remove several entities in the table of <typeref name="T" />
	/// </summary>
	/// <param name="entities"><see cref="IEnumerable{T}" /> of entity to remove</param>
	/// <returns></returns>
	public void RemoveRange(IEnumerable<T> entities)
	{
		Context.Set<T>().RemoveRange(entities);
	}

	/// <summary>
	///     Update an entity in the table of <typeref name="T" /> and returns the updated entity
	/// </summary>
	/// <param name="entity">Entity to updated, null attribute will not change</param>
	/// <returns>The updated entity <see cref="T" /></returns>
	public T Update(T entity)
	{
		Context.Set<T>().Update(entity);
		return entity;
	}

	/// <summary>
	///     Updates a list of entities in the table of <typeref name="T" /> and returns the updated entities
	/// </summary>
	/// <param name="entities">Entities to be updated, null attributes will NOT change</param>
	/// <returns>The updated entities <see cref="T" /></returns>
	public T[] UpdateArray(T[] entities)
	{
		foreach (T entity in entities)
			Context.Set<T>().Update(entity);
		return entities;
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
		IQueryable<T> query = Context.Set<T>().AsQueryable();
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