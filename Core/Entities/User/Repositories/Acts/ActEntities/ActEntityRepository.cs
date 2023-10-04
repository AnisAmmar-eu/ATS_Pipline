using Core.Entities.User.Models.DB.Acts;
using Core.Entities.User.Models.DB.Acts.ActEntities;
using Core.Shared.Data;
using Core.Shared.Exceptions;
using Core.Shared.Repositories.Kernel;
using System.Linq.Expressions;
using Core.Entities.User.Models.DTO.Acts.ActEntities;

namespace Core.Entities.User.Repositories.Acts.ActEntities
{
	public class ActEntityRepository : RepositoryBaseEntity<AlarmCTX, ActEntity, DTOActEntity>, IActEntityRepository
	{
		public ActEntityRepository(AlarmCTX context) : base(context)
		{
		}

		private static Expression<Func<ActEntity, bool>>[] QueryFilters(Act act, int? entityID = null, int? parentID = null)
		{
			IEnumerable<Expression<Func<ActEntity, bool>>> filters = new Expression<Func<ActEntity, bool>>[] { ae =>
				ae.Act.RID == act.RID
				&& ae.Act.EntityType == act.EntityType
				&& ae.Act.ParentType == act.ParentType
				&& ae.EntityID == entityID
				&& ae.ParentID == parentID
			};

			return filters.ToArray();
		}

		public async Task<List<ActEntity>> GetAllByActWithIncludes(Act act, int? entityID, int? parentID, bool withTracking = true, int? maxCount = null)
		{
			Expression<Func<ActEntity, bool>>[] filters = QueryFilters(act, entityID, parentID);

			return await GetAll(
				filters: filters,
				orderBy: null,
				withTracking: withTracking,
				maxCount: maxCount,
				"Act.ActLangs.Language",
				"SignatureType.C_Langs.Language",
				"ActEntityRoles"
			);
		}

		public async Task<ActEntity> GetByActWithIncludes(Act act, int? entityID, int? parentID, bool withTracking = true)
		{
			Expression<Func<ActEntity, bool>>[] filters = QueryFilters(act, entityID, parentID);

			return await GetBy(
				filters,
				null,
				withTracking,
				"ActEntityRoles"
			);
		}
	}
}
