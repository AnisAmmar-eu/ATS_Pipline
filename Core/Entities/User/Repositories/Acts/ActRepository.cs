﻿using System.Linq.Expressions;
using Core.Entities.User.Models.DB.Acts;
using Core.Entities.User.Models.DTO.Acts;
using Core.Shared.Data;
using Core.Shared.Exceptions;
using Core.Shared.Repositories.Kernel;

namespace Core.Entities.User.Repositories.Acts;

public class ActRepository : RepositoryBaseEntity<AlarmCTX, Act, DTOAct>, IActRepository
{
	public ActRepository(AlarmCTX context) : base(context)
	{
	}


	public async Task<Act> GetByRIDAndTypeWithIncludes(string? rid, string? entityType, string? parentType,
		bool withTracking = true)
	{
		if (rid == null)
			throw new EntityNotFoundException("Can't find an action with no RID.");

		Expression<Func<Act, bool>>[] filters = QueryFilters(rid, entityType, parentType);

		return await GetBy(
			filters,
			withTracking: withTracking
		);
	}

	private static Expression<Func<Act, bool>>[] QueryFilters(string rid, string? entityType = null,
		string? parentType = null)
	{
		IEnumerable<Expression<Func<Act, bool>>> filters = new Expression<Func<Act, bool>>[]
		{
			a =>
				a.RID == rid
				&& a.EntityType == entityType
				&& a.ParentType == parentType
		};

		return filters.ToArray();
	}
}