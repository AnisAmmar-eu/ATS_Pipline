using System.Linq.Expressions;
using Core.Shared.Exceptions;
using Core.Shared.Models.DB.Kernel.Interfaces;
using Core.Shared.Models.DTO.Kernel.Interfaces;
using Core.Shared.Paginations;
using Core.Shared.Paginations.Filtering;
using Core.Shared.Paginations.Sorting;
using Core.Shared.Paginations.TextSearches;
using Core.Shared.Repositories.Kernel.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Core.Shared.Repositories.Kernel;

/// <summary>
///     Implements the <see cref="IBaseEntityRepository{T,TDTO}" /> interface
/// </summary>
/// <typeparam name="TContext"> <see cref="DbContext" /> of the project</typeparam>
/// <typeparam name="T">
///     Type that defines a table in the database and have to implement <see cref="IBaseEntity{T}" />
/// </typeparam>
/// <typeparam name="TDTO"></typeparam>
public class BaseEntityRepository<TContext, T, TDTO> : IBaseEntityRepository<T, TDTO>
	where TContext : DbContext
	where T : class, IBaseEntity<T, TDTO>
	where TDTO : class, IDTO<T, TDTO>
{
	private readonly ICollection<Expression<Func<T, bool>>> _importFilters = new List<Expression<Func<T, bool>>>();
	protected readonly TContext Context;

	/// <summary>
	///     Constructor
	/// </summary>
	/// <param name="context"><see cref="DbContext" /> of the project</param>
	protected BaseEntityRepository(TContext context)
	{
		Context = context;
	}

	/// <summary>
	///     Get an entity based on ID from the table of <typeref name="T" /> with join to its navigation properties
	/// </summary>
	/// <param name="id"></param>
	/// <param name="filters"></param>
	/// <param name="withTracking"></param>
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
			includes: new Dictionary<string, string[]> { { string.Empty, includes } }
				)
			.FirstOrDefaultAsync(x => x.ID == id);
		if (t is null)
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
				)
			.FirstOrDefaultAsync(x => x.ID == id);
		if (t is null)
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
		T? t = await Query(
			filters,
			orderBy,
			withTracking,
			includes: new Dictionary<string, string[]> { { string.Empty, includes } })
			.FirstOrDefaultAsync();
		if (t is null)
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
		if (t is null)
			throw new EntityNotFoundException(typeof(T).Name + " not found");

		return t;
	}

	/// <summary>
	///     Get all entities from the table of <typeref name="T" /> with join to its navigation properties
	/// </summary>
	/// <param name="filters"></param>
	/// <param name="orderBy"></param>
	/// <param name="withTracking"></param>
	/// <param name="maxCount"></param>
	/// <param name="includes">Variadic parameter, array of <see cref="string" /> of names of column to include in the query</param>
	public Task<List<T>> GetAll(
		Expression<Func<T, bool>>[]? filters = null,
		Func<IQueryable<T>, IOrderedQueryable<T>>? orderBy = null,
		bool withTracking = true,
		int? maxCount = null,
		params string[] includes
		)
	{
		//return await Query(filters, orderBy, withTracking, maxCount).ToListAsync();
		return Query(
			filters,
			orderBy,
			withTracking,
			maxCount,
			new Dictionary<string, string[]> { { string.Empty, includes } })
			.ToListAsync();
	}

	public Task<List<T>> GetAllWithConcat(
		Expression<Func<T, bool>>[]? filters = null,
		Func<IQueryable<T>, IOrderedQueryable<T>>? orderBy = null,
		bool withTracking = true,
		int? maxCount = null,
		Dictionary<string, string[]>? includes = null
		)
	{
		return Query(filters, orderBy, withTracking, maxCount, includes).ToListAsync();
	}

	public Task<List<T>> GetWithPagination(Pagination pagination, int nbItems)
	{
		// No split query because we are using .Take();
		// No tracking as this is used for back to front purposes and thus useless.
		// First line is aggregating every include
		IOrderedQueryable<T> query = pagination.Includes
			.Aggregate(
				Context.Set<T>()
					.AsQueryable(),
				(current, value) => current.Include(value))
			.AsNoTracking()
			.FilterFromPagination<T, TDTO>(pagination)
			.TextSearchFromPagination<T, TDTO>(pagination)
			.SortFromPagination<T, TDTO>(pagination);

		return (nbItems == 0) ? query.ToListAsync() : query.Take(nbItems).ToListAsync();
	}

	public Task<int> CountWithPagination(Pagination pagination)
	{
		return pagination.Includes
			.Aggregate(
				Context.Set<T>()
					.AsQueryable(),
				(current, value) => current.Include(value))
			.AsNoTracking()
			.FilterFromPagination<T, TDTO>(pagination)
			.TextSearchFromPagination<T, TDTO>(pagination)
			.CountAsync();
	}

	/// <summary>
	///     Find entities by a predicate
	/// </summary>
	/// <param name="expression">Predicate</param>
	public Task<List<T>> Find(Expression<Func<T, bool>> expression)
	{
		return Context.Set<T>().Where(expression).ToListAsync();
	}

	/// <summary>
	///     Add an new entity in the table of <typeref name="T" />
	/// </summary>
	/// <param name="entity"></param>
	public async Task Add(T entity)
	{
		await Context.Set<T>().AddAsync(entity);
	}

	/// <summary>
	///     Add an new entity in the table of <typeref name="T" /> and return the new entity as <see cref="IDTO" />
	/// </summary>
	/// <param name="entity"></param>
	/// <returns>The entity <see cref="T" /> saved in the database</returns>
	public async Task<T> AddAndReturn(T entity)
	{
		await Context.Set<T>().AddAsync(entity);
		return entity;
	}

	/// <summary>
	///     Add several entities in the table of <typeref name="T" />
	/// </summary>
	/// <param name="entities"></param>
	public Task AddRange(IEnumerable<T> entities)
	{
		return Context.Set<T>().AddRangeAsync(entities);
	}

	/// <summary>
	///     Remove an entity in the table of <typeref name="T" />
	/// </summary>
	/// <param name="entity">The entity <see cref="T" /> to remove</param>
	public void Remove(T entity)
	{
		Context.Set<T>().Remove(entity);
	}

	/// <summary>
	///     Remove an entitiy in the table of <typeref name="T" /> with the given ID
	/// </summary>
	/// <param name="id">ID of the entity to remove</param>
	/// <param name="includes"></param>
	public async Task Remove(int id, params string[] includes)
	{
		IQueryable<T> query = Context.Set<T>().AsQueryable();
		query = includes.Aggregate(query, (current, include) => current.Include(include));
		if (includes.Length != 0)
			query = query.AsNoTracking();

		T? entity = await query.FirstOrDefaultAsync(x => x.ID == id);
		if (entity is null)
			throw new EntityNotFoundException(typeof(T).Name, id);

		Context.Set<T>().Remove(entity);
	}

	/// <summary>
	///	Removes all entities outside the specified lifespan in a bulk delete.
	/// e.g. with a lifespan of 10 days, all rows older than that will be deleted.
	/// </summary>
	/// <param name="lifeSpan"></param>
	/// <returns></returns>
	public Task RemoveByLifeSpan(TimeSpan lifeSpan)
	{
		DateTimeOffset threshold = DateTimeOffset.Now.Subtract(lifeSpan);
		return Context.Set<T>().AsQueryable().Where(t => t.TS < threshold).ExecuteDeleteAsync();
	}

	/// <summary>
	///     Remove several entities in the table of <typeref name="T" />
	/// </summary>
	/// <param name="entities"><see cref="IEnumerable{T}" /> of entity to remove</param>
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
	/// <param name="withTracking"></param>
	/// <param name="includes"></param>
	public Task<bool> Any(Expression<Func<T, bool>> predicate, bool withTracking = true, params string[] includes)
	{
		return Query(
			[predicate],
			null,
			withTracking,
			null,
			new Dictionary<string, string[]> { { string.Empty, includes } }
				)
			.AnyAsync(predicate);
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
		if (includes is not null)
		{
			{
				foreach (KeyValuePair<string, string[]> include in includes)
				{
					if (include.Key != string.Empty)
					{
						query = query.Include(include.Key);
						query = include.Value.Aggregate(
							query,
							(current, value) => current.Include(include.Key + "." + value));
					}
					else
					{
						query = include.Value.Aggregate(query, (current, value) => current.Include(value));
					}
				}

				if (includes.Count > 0)
				{
					// WARNING - https://learn.microsoft.com/fr-fr/ef/core/querying/single-split-queries
					query = query.AsSplitQuery();
				}
			}
		}

		if (!withTracking)
			query = query.AsNoTracking();

		if (_importFilters.Count > 0)
			query = _importFilters.Aggregate(query, (current, filter) => current.Where(filter));

		if (filters is not null)
			query = filters.Aggregate(query, (current, filter) => current.Where(filter));

		if (maxCount is not null)
			query = query.Take(maxCount.Value);

		if (orderBy is not null)
			return orderBy(query);

		return query;
	}
}