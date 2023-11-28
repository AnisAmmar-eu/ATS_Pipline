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
public interface IBaseEntityRepository<T, TDTO>
	where T : class, IBaseEntity<T, TDTO>
	where TDTO : class, IDTO<T, TDTO>
{
	/*/// <summary>
	///     Get entity by id from the table of <typeref name="T"/>
	/// </summary>
	/// <param name="id">ID of the entity to get</param>
	/// <returns>The entity <see cref="T"/></returns>
	Task<T> GetById(int id);*/

	/// <summary>
	///     Get an entity based on ID from the table of <typeref name="T" /> with join to its navigation properties
	/// </summary>
	/// <param name="id"></param>
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
	Task<T> GetBy(
		Expression<Func<T, bool>>[]? filters = null,
		Func<IQueryable<T>, IOrderedQueryable<T>>? orderBy = null,
		bool withTracking = true,
		params string[] includes
	);

	Task<T> GetByWithConcat(
		Expression<Func<T, bool>>[]? filters = null,
		Func<IQueryable<T>, IOrderedQueryable<T>>? orderBy = null,
		bool withTracking = true,
		Dictionary<string, string[]>? includes = null
	);


	/*	/// <summary>
	///     Get all entities from the table of <typeref name="T"/>
	/// </summary>
	/// <returns>List of entities <see cref="List{T}"/></returns>
	Task<List<T>> GetAll();*/

	/// <summary>
	///     Get all entities from the table of <typeref name="T" /> with join to its navigation properties
	/// </summary>
	/// <param name="includes">Variadic parameter, array of <see cref="string" /> of names of column to include in the query</param>
	/// <returns></returns>
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

	Task<List<T>> GetWithPagination(Pagination pagination, int nbItems, int lastID);

	/// <summary>
	///     Find entities by a predicate
	/// </summary>
	/// <param name="expression">Predicate</param>
	/// <returns></returns>
	Task<List<T>> Find(Expression<Func<T, bool>> expression);

	/// <summary>
	///     Add an new entity in the table of <typeref name="T" />
	/// </summary>
	/// <param name="dto"><see cref="TDTO{T}" /> dto to use to instantiate the new entity</param>
	/// <returns></returns>
	Task Add(T entity);

	/// <summary>
	///     Add an new entity in the table of <typeref name="T" /> and return the new entity as <see cref="IDTO" />
	/// </summary>
	/// <param name="dto"></param>
	/// <returns>The entity <see cref="T" /> saved in the database</returns>
	Task<T> AddAndReturn(T entity);

	/// <summary>
	///     Add several entities in the table of <typeref name="T" />
	/// </summary>
	/// <param name="entities"><see cref="IEnumerable{T}" /> of entity to instantiate in the db</param>
	/// <returns></returns>
	Task AddRange(IEnumerable<T> entities);

	/// <summary>
	///     Remove an entity in the table of <typeref name="T" />
	/// </summary>
	/// <param name="entity">The entity <see cref="T" /> to remove</param>
	/// <returns></returns>
	void Remove(T entity);

	/// <summary>
	///     Remove an entity in the table of <typeref name="T" /> with the given ID
	/// </summary>
	/// <param name="id">ID of the entity to remove</param>
	/// <param name="includes"></param>
	/// <returns></returns>
	Task Remove(int id, params string[] includes);

	/// <summary>
	///     Remove several entities in the table of <typeref name="T" />
	/// </summary>
	/// <param name="entities"><see cref="IEnumerable{T}" /> of entity to remove</param>
	/// <returns></returns>
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
	///     Check if an element exist with the predication
	/// </summary>
	/// <param name="predicate"></param>
	/// <param name="includes"></param>
	/// <returns></returns>
	Task<bool> Any(Expression<Func<T, bool>> predicate, bool withTracking = true, params string[] includes);
}