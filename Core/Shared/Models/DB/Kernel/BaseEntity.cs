﻿using Core.Shared.Models.DB.Kernel.Interfaces;
using Core.Shared.Models.DTO.Kernel;
using Mapster;

namespace Core.Shared.Models.DB.Kernel;

/// <summary>
///     Base class for all entities
/// </summary>
public class BaseEntity : IBaseEntity<BaseEntity, DTOBaseEntity>
{
	public BaseEntity()
	{
	}

	protected BaseEntity(DTOBaseEntity dto)
	{
		ID = dto.ID;
		TS = (DateTimeOffset)dto.TS!;
	}

	[AdaptIgnore(MemberSide.Destination)]
	public int ID { get; set; }

	public DateTimeOffset TS { get; set; } = DateTimeOffset.Now;

	/// <summary>
	///     Converts the entity to its DTO.
	/// </summary>
	/// <returns> <see cref="DTO" /> of the entity</returns>
	public virtual DTOBaseEntity ToDTO()
	{
		return new() {
			ID = ID,
			TS = TS,
		};
	}
}