using System.Linq.Expressions;
using Core.Shared.Models.DB.Kernel.Interfaces;
using Core.Shared.Models.DTO.Kernel.Interfaces;

namespace Core.Shared.Repositories.Kernel.Interfaces;

/// <summary>
///     Base repository interface
/// </summary>
/// <remarks>
///     The repository design pattern is only used for Location and Equipment entities yet.
/// </remarks>
/// <typeparam name="T">
///     Type of the entity to manipulate, should be in the base and defined in the namespace
///     <see cref="Entity" />
/// </typeparam>
public interface IRepositoryBase<T, TDTO>
	where T : class, IBaseEntity<T, TDTO>
	where TDTO : class, IDTO<T, TDTO>
{
	/// <summary>
	///     Get entity by id from the table of <typeref name="T" />
	/// </summary>
	/// <param name="id">ID of the entity to get</param>
	/// <returns><see cref="IDTO{T, DTO}" /> of the entity</returns>
	Task<IDTO<T, TDTO>> GetById(int id, string? languageRID = null);

	/// <summary>
	///     Get all entities from the table of <typeref name="T" /> with join to its navigation properties
	/// </summary>
	/// <param name="id"></param>
	/// <param name="includes">Variadic parameter, array of <see cref="string" /> of names of column to include in the query</param>
	/// <returns><see cref="IDTO{T, DTO}" /> of the entity</returns>
	Task<IDTO<T, TDTO>> GetById(int id, string? languageRID = null, params string[] includes);

	/// <summary>
	///     Get all entities from the table of <typeref name="T" />
	/// </summary>
	/// <returns><see cref="List{IDTO{T, DTO}}" /> of the entity</returns>
	Task<List<IDTO<T, TDTO>>> GetAll(string? languageRID = null);

	/// <summary>
	///     Get all entities from the table of <typeref name="T" /> with join to its navigation properties
	/// </summary>
	/// <param name="includes">Variadic parameter, array of <see cref="string" /> of names of column to include in the query</param>
	/// <returns></returns>
	Task<List<IDTO<T, TDTO>>> GetAll(string? languageRID = null, params string[] includes);

	/// <summary>
	///     Find entities by a predicate
	/// </summary>
	/// <param name="expression">Predicate</param>
	/// <returns></returns>
	Task<IEnumerable<IDTO<T, TDTO>>> Find(Expression<Func<IBaseEntity<T, TDTO>, bool>> expression,
		string? languageRID = null);

	/// <summary>
	///     Add an new entity in the table of <typeref name="T" />
	/// </summary>
	/// <param name="dto"><see cref="IDTO{T, DTO}" /> dto to use to instantiate the new entity</param>
	/// <returns></returns>
	Task Add(IDTO<T, TDTO> dto);

	/// <summary>
	///     Add an new entity in the table of <typeref name="T" /> and return the new entity as <see cref="IDTO{T, DTO}" />
	/// </summary>
	/// <param name="dto"></param>
	/// <returns>Return the <see cref="IDTO{T, DTO}" /> of the entity saved in the database</returns>
	Task<IDTO<T, TDTO>> AddAndReturn(IDTO<T, TDTO> dto, string? languageRID = null);

	/// <summary>
	///     Add several entities in the table of <typeref name="T" />
	/// </summary>
	/// <param name="dtos"><see cref="IEnumerable{IDTO{T, DTO}}" /> of DTO to instantiate in the db</param>
	/// <returns></returns>
	Task AddRange(IEnumerable<IDTO<T, TDTO>> dtos);

	/// <summary>
	///     Remove an entity in the table of <typeref name="T" />
	/// </summary>
	/// <param name="dto">dto of the entity to remove</param>
	/// <returns></returns>
	Task Remove(IDTO<T, TDTO> dto);

	/// <summary>
	///     Remove an entitiy in the table of <typeref name="T" /> with the given ID
	/// </summary>
	/// <param name="id">ID of the entity to remove</param>
	/// <returns></returns>
	Task Remove(int id);

	/// <summary>
	///     Remove several entities in the table of <typeref name="T" />
	/// </summary>
	/// <param name="dtos">DTOs enumerable of entity to remove</param>
	/// <returns></returns>
	Task RemoveRange(IEnumerable<IDTO<T, TDTO>> dtos);

	/// <summary>
	///     Update an entity in the table of <typeref name="T" /> and return the updated entity as DTO
	/// </summary>
	/// <param name="dto">DTO of the entity to updated, null attribute will not change</param>
	/// <returns>The <see cref="IDTO{T, DTO}" /> of the updated entity</returns>
	Task<IDTO<T, TDTO>> Update(IDTO<T, TDTO> dto, string? languageRID = null);
}