using Core.Shared.Data;

namespace Core.Entities.User.Repositories.Roles;

public class RoleRepository : IRoleRepository
{
	private readonly AnodeCTX _context;

	public RoleRepository(AnodeCTX context)
	{
		_context = context;
	}
}