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
/// <typeparam name="TDTO">
///     Type that defines a DTO of <typeref name="T" /> and have to implement <see cref="IDTO{T,TDTO}" />
/// </typeparam>
public class BaseEntityRepository<TContext, T, TDTO> : IBaseEntityRepository<T, TDTO>
	where TContext : DbContext
	where T : class, IBaseEntity<T, TDTO>
	where TDTO : class, IDTO<T, TDTO>
{
	protected readonly ICollection<Expression<Func<T, bool>>> _importFilters = [];
	private readonly string[] _baseIncludes;
	private readonly Dictionary<string, string[]> _baseConcatIncludes;

	protected readonly TContext _context;

	/// <summary>
	///     Constructor
	/// </summary>
	/// <param name="context"><see cref="DbContext" /> of the project</param>
	/// <param name="baseIncludes">All includes made in the ..WithIncludes methods</param>
	/// <param name="baseConcatIncludes">All includes made within foreign relations (eg: WorkingOrder.EquipmentI.EquipmentC)</param>
	public BaseEntityRepository(
		TContext context,
		string[] baseIncludes,
		Dictionary<string, string[]> baseConcatIncludes)
	{
		_context = context;
		_baseIncludes = baseIncludes;
		_baseConcatIncludes = baseConcatIncludes;
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
			includes: new Dictionary<string, string[]> { { string.Empty, _baseIncludes.Concat(includes).ToArray() } }
				)
			.FirstOrDefaultAsync(x => x.ID == id);
		return t ?? throw new EntityNotFoundException(typeof(T).Name, id);
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
			includes: GetMergedIncludes(includes))
			.FirstOrDefaultAsync(x => x.ID == id);
		return t ?? throw new EntityNotFoundException(typeof(T).Name, id);
	}

	/// <summary>
	///     Get an entity from the table of <typeref name="T" /> with join to its navigation properties
	/// </summary>
	/// <param name="filters"></param>
	/// <param name="orderBy"></param>
	/// <param name="withTracking"></param>
	/// <param name="includes"></param>
	/// <returns>The entity <see cref="T" /></returns>
	public async Task<T> GetByWithThrow(
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
			includes: new Dictionary<string, string[]> { { string.Empty, _baseIncludes.Concat(includes).ToArray() } })
			.FirstOrDefaultAsync();
		return t ?? throw new EntityNotFoundException(typeof(T).Name + " not found");
	}

	public Task<T?> GetBy(
		Expression<Func<T, bool>>[]? filters = null,
		Func<IQueryable<T>, IOrderedQueryable<T>>? orderBy = null,
		bool withTracking = true,
		params string[] includes
	)
	{
		return Query(
			filters,
			orderBy,
			withTracking,
			includes: new Dictionary<string, string[]> { { string.Empty, _baseIncludes.Concat(includes).ToArray() } })
			.FirstOrDefaultAsync();
	}

	public async Task<T> GetByWithConcatWithThrow(
		Expression<Func<T, bool>>[]? filters = null,
		Func<IQueryable<T>, IOrderedQueryable<T>>? orderBy = null,
		bool withTracking = true,
		Dictionary<string, string[]>? includes = null
		)
	{
		T? t = await Query(filters, orderBy, withTracking, includes: GetMergedIncludes(includes)).FirstOrDefaultAsync();
		return t ?? throw new EntityNotFoundException(typeof(T).Name + " not found");
	}

	public Task<T?> GetByWithConcat(
		Expression<Func<T, bool>>[]? filters = null,
		Func<IQueryable<T>, IOrderedQueryable<T>>? orderBy = null,
		bool withTracking = true,
		Dictionary<string, string[]>? includes = null
) => Query(filters, orderBy, withTracking, includes: GetMergedIncludes(includes)).FirstOrDefaultAsync();

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
			new Dictionary<string, string[]> { { string.Empty, _baseIncludes.Concat(includes).ToArray() } })
			.ToListAsync();
	}

	public Task<List<T>> GetAllWithConcat(
		Expression<Func<T, bool>>[]? filters = null,
		Func<IQueryable<T>, IOrderedQueryable<T>>? orderBy = null,
		bool withTracking = true,
		int? maxCount = null,
		Dictionary<string, string[]>? includes = null
		) => Query(filters, orderBy, withTracking, maxCount, includes).ToListAsync();

	public Task<List<T>> GetWithPagination(Pagination pagination, int nbItems)
	{
		// No split query because we are using .Take();
		// No tracking as this is used for back to front purposes and thus useless.
		// First line is aggregating every include
		IOrderedQueryable<T> query = QueryIncludes(
			_context.Set<T>().AsQueryable(),
			GetMergedIncludes(new() { { string.Empty, _baseIncludes.Concat(pagination.Includes).ToArray() } }))
			.AsNoTracking()
			.FilterFromPagination<T, TDTO>(pagination)
			.TextSearchFromPagination<T, TDTO>(pagination)
			.SortFromPagination<T, TDTO>(pagination);

		return (nbItems <= 0) ? query.ToListAsync() : query.Take(nbItems).ToListAsync();
	}

	public Task<int> CountWithPagination(Pagination pagination)
	{
		return pagination.Includes
			.Aggregate(
				_context.Set<T>()
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
	public Task<List<T>> Find(Expression<Func<T, bool>> expression) => _context.Set<T>().Where(expression).ToListAsync();

	/// <summary>
	///     Add an new entity in the table of <typeref name="T" />
	/// </summary>
	/// <param name="entity"></param>
	public async Task Add(T entity) => _ = await _context.Set<T>().AddAsync(entity);

	/// <summary>
	///     Add an new entity in the table of <typeref name="T" /> and return the new entity as <see cref="IDTO" />
	/// </summary>
	/// <param name="entity"></param>
	/// <returns>The entity <see cref="T" /> saved in the database</returns>
	public async Task<T> AddAndReturn(T entity)
	{
		_ = await _context.Set<T>().AddAsync(entity);
		return entity;
	}

	/// <summary>
	///     Add several entities in the table of <typeref name="T" />
	/// </summary>
	/// <param name="entities"></param>
	public Task AddRange(IEnumerable<T> entities) => _context.Set<T>().AddRangeAsync(entities);

	/// <summary>
	///     Remove an entity in the table of <typeref name="T" />
	/// </summary>
	/// <param name="entity">The entity <see cref="T" /> to remove</param>
	public void Remove(T entity) => _ = _context.Set<T>().Remove(entity);

	/// <summary>
	///     Remove an entitiy in the table of <typeref name="T" /> with the given ID
	/// </summary>
	/// <param name="id">ID of the entity to remove</param>
	/// <param name="includes"></param>
	public async Task Remove(int id, params string[] includes)
	{
		IQueryable<T> query = _context.Set<T>().AsQueryable();
		query = _baseIncludes.Concat(includes).Aggregate(query, (current, include) => current.Include(include));
		if (includes.Length != 0)
			query = query.AsNoTracking();

		T? entity = await query.FirstOrDefaultAsync(x => x.ID == id) ?? throw new EntityNotFoundException(typeof(T).Name, id);
		_ = _context.Set<T>().Remove(entity);
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
		return _context.Set<T>().AsQueryable().Where(t => t.TS < threshold).ExecuteDeleteAsync();
	}

	/// <summary>
	///     Remove several entities in the table of <typeref name="T" />
	/// </summary>
	/// <param name="entities"><see cref="IEnumerable{T}" /> of entity to remove</param>
	public void RemoveRange(IEnumerable<T> entities) => _context.Set<T>().RemoveRange(entities);

	/// <summary>
	///     Update an entity in the table of <typeref name="T" /> and returns the updated entity
	/// </summary>
	/// <param name="entity">Entity to updated, null attribute will not change</param>
	/// <returns>The updated entity <see cref="T" /></returns>
	public T Update(T entity)
	{
		_ = _context.Set<T>().Update(entity);
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
			_ = _context.Set<T>().Update(entity);

		return entities;
	}

	// Update using ExecuteUpdateAsync

	/// <summary>
	///    Update an entity in the table of <typeref name="T" /> and returns the updated entity
	///    using the ExecuteUpdateAsync method
	///    SetProperties is used to update only the properties that are not null
	/// </summary>
	/// <param name="entity">Entity to updated, null attribute will not change</param>
	/// <param name="properties">Properties to update</param>
	/// <returns>The updated entity <see cref="T" /></returns>
	public Task<int> ExecuteUpdateByIdAsync(
		T entity,
		Expression<Func<
			Microsoft.EntityFrameworkCore.Query.SetPropertyCalls<T>,
			Microsoft.EntityFrameworkCore.Query.SetPropertyCalls<T>>> properties)
				=> _context.Set<T>().Where(x => x.ID == entity.ID).ExecuteUpdateAsync(properties);

	/// <summary>
	///    Update an entity in the table of <typeref name="T" /> and returns the updated entity
	///    using the ExecuteUpdateAsync method
	///    SetProperties is used to update only the properties that are not null
	/// </summary>
	/// <param name="predicate">Predicate where</param>
	/// <param name="properties">Properties to update</param>
	/// <returns>The updated entity <see cref="T" /></returns>
	public Task<int> ExecuteUpdateAsync(
		Expression<Func<T, bool>> predicate,
		Expression<Func<
			Microsoft.EntityFrameworkCore.Query.SetPropertyCalls<T>,
			Microsoft.EntityFrameworkCore.Query.SetPropertyCalls<T>>> properties)
				=> _context.Set<T>().Where(predicate).ExecuteUpdateAsync(properties);

	/// <summary>
	///    Delete an entity in the table of <typeref name="T" /> and returns the number entity affected
	///    using the ExecuteDeleteAsync method
	/// </summary>
	/// <param name="predicate">Predicate where</param>
	/// <returns>The deleted entity <see cref="T" /></returns>
	public Task<int> ExecuteDeleteAsync(
		Expression<Func<T, bool>> predicate) => _context.Set<T>().Where(predicate).ExecuteDeleteAsync();

	/// <summary>
	///     Check if an element exist with the predication
	/// </summary>
	/// <param name="predicate"></param>
	/// <param name="withTracking"></param>
	/// <param name="includes"></param>
	public Task<bool> AnyPredicate(Expression<Func<T, bool>> predicate, bool withTracking = true, params string[] includes)
	{
		return Query(
			[predicate],
			null,
			withTracking,
			null,
			new Dictionary<string, string[]> { { string.Empty, _baseIncludes.Concat(includes).ToArray() } }
				)
			.AnyAsync(predicate);
	}

	public Task<bool> Any(bool withTracking = true, params string[] includes)
	{
		return Query(
			[],
			null,
			withTracking,
			null,
			new Dictionary<string, string[]> { { string.Empty, _baseIncludes.Concat(includes).ToArray() } }
				)
			.AnyAsync();
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
		if (includes is not null)
			query = QueryIncludes(query, includes);

		if (!withTracking)
			query = query.AsNoTracking();

		if (_importFilters.Count > 0)
			query = _importFilters.Aggregate(query, (current, filter) => current.Where(filter));

		if (filters is not null)
			query = filters.Aggregate(query, (current, filter) => current.Where(filter));

		if (maxCount is not null)
			query = query.Take(maxCount.Value);

		return (orderBy is not null) ? orderBy(query) : query;
	}

	private static IQueryable<T> QueryIncludes(IQueryable<T> query, Dictionary<string, string[]> includes)
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

		return query;
	}

	private Dictionary<string, string[]> GetMergedIncludes(Dictionary<string, string[]>? includes)
	{
		// Here, we copy the dictionary as user-defined includes should override baseConcatIncludes when needed and not otherwise.
		Dictionary<string, string[]> mergedIncludes
			= _baseConcatIncludes.ToDictionary(entry => entry.Key, entry => entry.Value);
		mergedIncludes.Add(string.Empty, _baseIncludes);
		foreach ((string? key, string[]? value) in includes ?? [])
		{
			bool isExist = mergedIncludes.TryGetValue(key, out string[]? existingValue);
			mergedIncludes[key] = isExist
				? [.. existingValue, .. value]
				: value;
		}

		return mergedIncludes;
	}
}