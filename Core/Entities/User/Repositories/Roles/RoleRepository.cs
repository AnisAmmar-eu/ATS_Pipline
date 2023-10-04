using Core.Shared.Data;

namespace Core.Entities.User.Repositories.Roles;

public class RoleRepository : IRoleRepository
{
	private readonly AlarmCTX _context;

	public RoleRepository(AlarmCTX context)
	{
		_context = context;
	}
}