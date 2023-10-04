using Microsoft.AspNetCore.Mvc;

namespace Core.Shared.Authorize
{
	public class ActAuthorize : TypeFilterAttribute
	{
		public ActAuthorize(string rid, string entityType, string entityProperty, string parentType, string parentProperty) : base(typeof(ActAuthorizeLogic))
		{
			Arguments = new object[] { rid, entityType, entityProperty, parentType, parentProperty };
		}
		public ActAuthorize(string rid, string entityType, string entityProperty) : base(typeof(ActAuthorizeLogic))
		{
			Arguments = new object[] { rid, entityType, entityProperty };
		}
		public ActAuthorize(string rid) : base(typeof(ActAuthorizeLogic))
		{
			Arguments = new object[] { rid };
		}
	}
}
