using Microsoft.AspNetCore.Authorization;

namespace Core.Shared.Authorize;

public class ActAuthorize : IAuthorizationRequirement
{
	public ActAuthorize(string rid)
	{
		RID = rid;
	}

	public string RID { get; set; }
}