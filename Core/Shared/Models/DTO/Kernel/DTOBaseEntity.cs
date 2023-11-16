﻿using Core.Shared.Models.DB.Kernel;
using Core.Shared.Models.DTO.Kernel.Interfaces;

namespace Core.Shared.Models.DTO.Kernel;

/// <summary>
///     Base class for all DTOs
/// </summary>
public class DTOBaseEntity : IDTO<BaseEntity, DTOBaseEntity>
{
	public int ID { get; set; }

	public DateTimeOffset? TS { get; set; }

	public DTOBaseEntity()
	{
	}

	protected DTOBaseEntity(BaseEntity entity)
	{
		ID = entity.ID;
		TS = entity.TS;
	}

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