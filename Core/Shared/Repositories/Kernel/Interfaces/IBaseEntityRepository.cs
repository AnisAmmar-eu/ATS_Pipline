using System.Linq.Expressions;
using Core.Shared.Models.DB.Kernel.Interfaces;
using Core.Shared.Models.DTO.Kernel.Interfaces;
using Core.Shared.Paginations;

namespace Core.Shared.Repositories.Kernel.Interfaces;

/// <summary>
///     Base repository entity interface
/// </summary>
/// <remarks>
///     The repository design pattern is only used for Location and Equipment entities yet.
/// </remarks>
/// <typeparam name="T">
///     Type of the entity to manipulate, should be in the base and defined in the namespace
///     <see cref="Entity" />
/// </typeparam>
/// <typeparam name="TDTO"></typeparam>
public interface IBaseEntityRepository<T, TDTO>
	where T : class, IBaseEntity<T, TDTO>
	where TDTO : class, IDTO<T, TDTO>
{
	/// <summary>
	///     Get an entity based on ID from the table of <typeref name="T" /> with join to its navigation properties
	/// </summary>
	/// <param name="id"></param>
	/// <param name="filters"></param>
	/// <param name="withTracking"></param>
	/// <param name="includes">Variadic parameter, array of <see cref="string" /> of names of column to include in the query</param>
	/// <returns>The entity <see cref="T" /></returns>
	Task<T> GetById(
		int id,
		Expression<Func<T, bool>>[]? filters = null,
		bool withTracking = true,
		params string[] includes
		);

	Task<T> GetByIdWithConcat(
		int id,
		Expression<Func<T, bool>>[]? filters = null,
		bool withTracking = true,
		Dictionary<string, string[]>? includes = null
		);

	/// <summary>
	///     Get an entity from the table of <typeref name="T" /> with join to its navigation properties
	/// </summary>
	/// <param name="filters"></param>
	/// <param name="orderBy"></param>
	/// <param name="withTracking"></param>
	/// <param name="includes"></param>
	/// <returns>The entity <see cref="T" /></returns>
	Task<T> GetByWithThrow(
		Expression<Func<T, bool>>[]? filters = null,
		Func<IQueryable<T>, IOrderedQueryable<T>>? orderBy = null,
		bool withTracking = true,
		params string[] includes
		);

	public Task<T?> GetBy(
		Expression<Func<T, bool>>[]? filters = null,
		Func<IQueryable<T>, IOrderedQueryable<T>>? orderBy = null,
		bool withTracking = true,
		params string[] includes);

	Task<T> GetByWithConcatWithThrow(
		Expression<Func<T, bool>>[]? filters = null,
		Func<IQueryable<T>, IOrderedQueryable<T>>? orderBy = null,
		bool withTracking = true,
		Dictionary<string, string[]>? includes = null
		);

	public Task<T?> GetByWithConcat(
		Expression<Func<T, bool>>[]? filters = null,
		Func<IQueryable<T>, IOrderedQueryable<T>>? orderBy = null,
		bool withTracking = true,
		Dictionary<string, string[]>? includes = null);

	/// <summary>
	///     Get all entities from the table of <typeref name="T" /> with join to its navigation properties
	/// </summary>
	/// <param name="filters"></param>
	/// <param name="orderBy"></param>
	/// <param name="withTracking"></param>
	/// <param name="maxCount"></param>
	/// <param name="includes">Variadic parameter, array of <see cref="string" /> of names of column to include in the query</param>
	Task<List<T>> GetAll(
		Expression<Func<T, bool>>[]? filters = null,
		Func<IQueryable<T>, IOrderedQueryable<T>>? orderBy = null,
		bool withTracking = true,
		int? maxCount = null,
		params string[] includes
		);

	Task<List<T>> GetAllWithConcat(
		Expression<Func<T, bool>>[]? filters = null,
		Func<IQueryable<T>, IOrderedQueryable<T>>? orderBy = null,
		bool withTracking = true,
		int? maxCount = null,
		Dictionary<string, string[]>? includes = null
		);

	/// <summary>
	///		Get a list of entities using pagination, filtering and sorting.
	/// </summary>
	/// <param name="pagination"></param>
	/// <param name="nbItems"></param>
	Task<List<T>> GetWithPagination(Pagination pagination, int nbItems);

	/// <summary>
	///		Returns the number of entities which are eligible to the filter and textSearch
	/// </summary>
	/// <param name="pagination"></param>
	Task<int> CountWithPagination(Pagination pagination);

	/// <summary>
	///     Find entities by a predicate
	/// </summary>
	/// <param name="expression">Predicate</param>
	Task<List<T>> Find(Expression<Func<T, bool>> expression);

	/// <summary>
	///     Add an new entity in the table of <typeref name="T" />
	/// </summary>
	/// <param name="entity"></param>
	Task Add(T entity);

	/// <summary>
	///     Add an new entity in the table of <typeref name="T" /> and return the new entity as <see cref="IDTO" />
	/// </summary>
	/// <param name="entity"></param>
	/// <returns>The entity <see cref="T" /> saved in the database</returns>
	Task<T> AddAndReturn(T entity);

	/// <summary>
	///     Add several entities in the table of <typeref name="T" />
	/// </summary>
	/// <param name="entities"><see cref="IEnumerable{T}" /> of entity to instantiate in the db</param>
	Task AddRange(IEnumerable<T> entities);

	/// <summary>
	///     Remove an entity in the table of <typeref name="T" />
	/// </summary>
	/// <param name="entity">The entity <see cref="T" /> to remove</param>
	void Remove(T entity);

	/// <summary>
	///     Remove an entity in the table of <typeref name="T" /> with the given ID
	/// </summary>
	/// <param name="id">ID of the entity to remove</param>
	/// <param name="includes"></param>
	Task Remove(int id, params string[] includes);

	/// <summary>
	///	Removes all entities outside the specified lifespan in a bulk delete.
	/// e.g. with a lifespan of 10 days, all rows older than that will be deleted.
	/// </summary>
	/// <param name="lifeSpan"></param>
	/// <returns></returns>
	public Task RemoveByLifeSpan(TimeSpan lifeSpan);

	/// <summary>
	///     Remove several entities in the table of <typeref name="T" />
	/// </summary>
	/// <param name="entities"><see cref="IEnumerable{T}" /> of entity to remove</param>
	void RemoveRange(IEnumerable<T> entities);

	/// <summary>
	///     Update an entity in the table of <typeref name="T" /> and return the updated entity
	/// </summary>
	/// <param name="entity">Entity to updated, null attribute will not change</param>
	/// <returns>The updated entity <see cref="T" /></returns>
	T Update(T entity);

	/// <summary>
	///     Updates a list of entities in the table of <typeref name="T" /> and returns the updated entities
	/// </summary>
	/// <param name="entities">Entities to be updated, null attributes will NOT change</param>
	/// <returns>The updated entities <see cref="T" /></returns>
	T[] UpdateArray(T[] entities);

	/// <summary>
	///    Update an entity in the table of <typeref name="T" /> and returns the updated entity
	///    using the ExecuteUpdateAsync method
	///    SetProperties is used to update only the properties that are not null
	/// </summary>
	/// <param name="entity">Entity to updated, null attribute will not change</param>
	/// <param name="properties">Properties to update</param>
	/// <returns>The updated entity <see cref="T" /></returns>
	Task<int> ExecuteUpdateByIdAsync(
		T entity,
		Expression<Func<
			Microsoft.EntityFrameworkCore.Query.SetPropertyCalls<T>,
			Microsoft.EntityFrameworkCore.Query.SetPropertyCalls<T>>> properties);

	/// <summary>
	///    Update an entity in the table of <typeref name="T" /> and returns the number entity affected
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
			Microsoft.EntityFrameworkCore.Query.SetPropertyCalls<T>>> properties);

	/// <summary>
	///    Delete an entity in the table of <typeref name="T" /> and returns the number entity affected
	///    using the ExecuteDeleteAsync method
	/// </summary>
	/// <param name="predicate">Predicate where</param>
	/// <returns>The deleted entity <see cref="T" /></returns>
	public Task<int> ExecuteDeleteAsync(
		Expression<Func<T, bool>> predicate);

	/// <summary>
	///     Check if an element exist with the predication
	/// </summary>
	/// <param name="predicate"></param>
	/// <param name="withTracking"></param>
	/// <param name="includes"></param>
	Task<bool> AnyPredicate(Expression<Func<T, bool>> predicate, bool withTracking = true, params string[] includes);
	Task<bool> Any(bool withTracking = true, params string[] includes);
}