﻿using Core.Shared.Models.DB.Kernel.Interfaces;

namespace Core.Shared.Models.DTO.Kernel.Interfaces;

/// <summary>
///     Interface defining the properties of a DTO.
/// </summary>
/// <typeparam name="T">Type which the DTO is linked</typeparam>
/// <typeparam name="TDTO"></typeparam>
public interface IDTO<T, TDTO>
	where T : class, IBaseEntity<T, TDTO>
	where TDTO : class, IDTO<T, TDTO>
{
	public int ID { get; set; }
	public DateTimeOffset? TS { get; set; }

	/// <summary>
	///     Converts the DTO to the entity as a <see cref="BaseEntity{T}" />.
	/// </summary>
	public IBaseEntity<T, TDTO> ToEntity()
	{
		return ToModel();
	}

	/// <summary>
	///     Converts the DTO to the model as its own type.
	/// </summary>
	T ToModel();
}