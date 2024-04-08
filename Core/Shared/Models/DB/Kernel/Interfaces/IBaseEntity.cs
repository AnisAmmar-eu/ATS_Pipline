using Core.Shared.Models.DTO.Kernel.Interfaces;
using Mapster;

namespace Core.Shared.Models.DB.Kernel.Interfaces;

/// <summary>
///     Interface defining the basic properties of an entity in the <see cref="AnodeCTX" />.
/// </summary>
/// <typeparam name="T"> The entity of the model</typeparam>
/// <typeparam name="TDTO"> The DTO of the entity. The DTO is used to communicate about an entity with the front end </typeparam>
public interface IBaseEntity<T, TDTO>
	where T : class, IBaseEntity<T, TDTO>
	where TDTO : class, IDTO<T, TDTO>
{
	[AdaptIgnore]
	public int ID { get; set; }

	public DateTimeOffset TS { get; set; }

	/// <summary>
	///     Returns the <see cref="IDTO{T,DTO}" /> of the entity T.
	/// </summary>
	public IDTO<T, TDTO> ToIDTO()
	{
		return ToDTO();
	}

	/*/// <summary>
	///    Converts the entity to its DTO.
	/// </summary>
	/// <returns> <see cref="DTO"/> of the entity</returns>
	DTO ToDTO();*/

	/// <summary>
	///     Converts the entity to its DTO.
	/// </summary>
	/// <returns> <see cref="TDTO" /> of the entity</returns>
	TDTO ToDTO();
}