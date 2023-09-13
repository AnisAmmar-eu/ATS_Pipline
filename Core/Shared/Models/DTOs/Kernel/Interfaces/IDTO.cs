using Core.Shared.Models.DB.Kernel.Interfaces;

namespace Core.Shared.Models.DTOs.Kernel.Interfaces;

/// <summary>
///     Interface defining the properties of a DTO.
/// </summary>
/// <typeparam name="T">Type which the DTO is linked</typeparam>
public interface IDTO<T, DTO>
	where T : class, IBaseEntity<T, DTO>
	where DTO : class, IDTO<T, DTO>
{
	public int ID { get; set; }
	public DateTimeOffset? TS { get; set; }

	// /// <summary>
	// ///     Converts the DTO to the entity as a <see cref="BaseEntity{T}" />.
	// /// </summary>
	// /// <returns></returns>
	// public IBaseEntity<T, DTO> ToEntity()
	// {
	//     return ToModel();
	// }

	// /// <summary>
	// ///     Converts the DTO to the model as its own type.
	// /// </summary>
	// /// <returns></returns>
	// T ToModel();
}