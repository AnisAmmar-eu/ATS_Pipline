using System.Linq.Expressions;
using Core.Entities.User.Models.DB.Acts;
using Core.Entities.User.Models.DTO.Acts;
using Core.Shared.Data;
using Core.Shared.Exceptions;
using Core.Shared.Repositories.Kernel;

namespace Core.Entities.User.Repositories.Acts;

public class ActRepository : BaseEntityRepository<AnodeCTX, Act, DTOAct>, IActRepository
{
	public ActRepository(AnodeCTX context) : base(context, [], [])
	{
	}

	public Task<Act> GetByRIDAndTypeWithIncludes(
		string? rid,
		string? entityType,
		string? parentType,
		bool withTracking = true)
	{
		if (rid is null)
			throw new EntityNotFoundException("Can't find an action with no RID.");

		Expression<Func<Act, bool>>[] filters = QueryFilters(rid, entityType, parentType);

		return GetByWithThrow(
			filters,
			withTracking: withTracking
		);
	}

	private static Expression<Func<Act, bool>>[] QueryFilters(
		string rid,
		string? entityType = null,
		string? parentType = null)
	{
		IEnumerable<Expression<Func<Act, bool>>> filters = [
			a =>
				a.RID == rid
					&& a.EntityType == entityType
					&& a.ParentType == parentType,
		];

		return filters.ToArray();
	}
}