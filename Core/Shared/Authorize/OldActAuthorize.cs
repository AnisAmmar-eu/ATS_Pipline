using Microsoft.AspNetCore.Mvc;

namespace Core.Shared.Authorize;

public class OldActAuthorize : TypeFilterAttribute
{
	public OldActAuthorize(string rid, string entityType, string entityProperty, string parentType, string parentProperty)
		: base(typeof(OldActAuthorizeLogic))
	{
		Arguments = new object[] { rid, entityType, entityProperty, parentType, parentProperty };
	}

	public OldActAuthorize(string rid, string entityType, string entityProperty) : base(typeof(OldActAuthorizeLogic))
	{
		Arguments = new object[] { rid, entityType, entityProperty };
	}

	public OldActAuthorize(string rid) : base(typeof(OldActAuthorizeLogic))
	{
		Arguments = new object[] { rid };
	}
}