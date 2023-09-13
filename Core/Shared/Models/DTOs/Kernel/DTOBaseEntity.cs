using Core.Shared.Models.DB.Kernel;
using Core.Shared.Models.DTOs.Kernel.Interfaces;

namespace Core.Shared.Models.DTOs.Kernel;

/// <summary>
///     Base class for all DTOs
/// </summary>
public class DTOBaseEntity : IDTO<BaseEntity, DTOBaseEntity>
{
	public int ID { get; set; }

	public DateTimeOffset? TS { get; set; }

	/// <summary>
	///     Convert the DTO to its entity
	/// </summary>
	/// <returns> <see cref="T" /> of the DTO</returns>
	public virtual BaseEntity ToModel()
	{
		return new BaseEntity
		{
			ID = ID
		};
	}
}