﻿using Core.Entities.User.Models.DB.Roles;

namespace Core.Entities.User.Models.DTO.Roles;

public partial class DTORole
{
	public DTORole()
	{
	}

	public DTORole(ApplicationRole role)
	{
		RID = role.Name ?? string.Empty;
	}
}