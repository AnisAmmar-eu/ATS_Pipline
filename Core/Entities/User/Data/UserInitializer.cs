using Core.Entities.User.Dictionary;
using Core.Entities.User.Models.DB.Acts;
using Core.Entities.User.Models.DB.Acts.ActEntities;
using Core.Entities.User.Models.DB.Acts.ActEntities.ActEntityRoles;
using Core.Entities.User.Models.DB.Roles;
using Core.Entities.User.Models.DB.Users;
using Core.Shared.Data;
using Microsoft.AspNetCore.Identity;

namespace Core.Entities.User.Data;

public class UserInitializer
{
	public static async Task Initialize(AnodeCTX anodeCTX, UserManager<ApplicationUser> userManager)
	{
		#region Roles

		if (!anodeCTX.Roles.Any())
		{
			anodeCTX.Roles.Add(new ApplicationRole
			{
				Name = "Ekidi-Administrator",
				NormalizedName = "EKIDI-ADMINISTRATOR",
				Type = ApplicationRoleType.SYSTEM_EKIDI
			});
			anodeCTX.Roles.Add(new ApplicationRole
			{
				Name = "Ekium-Administrator",
				NormalizedName = "EKIUM-ADMINISTRATOR",
				Type = ApplicationRoleType.SYSTEM_EKIUM
			});
			anodeCTX.SaveChanges();
		}

		#endregion

		#region Users

		if (!anodeCTX.Users.Any())
		{
			// Create User
			ApplicationUser newUser = new()
			{
				UserName = "ekium-admin",
				Email = "ekium-admin@admin.com",
				Firstname = "ekium",
				Lastname = "admin",
				IsEkium = true,
				Source = SourceAuth.EKIDI
			};

			await userManager.CreateAsync(newUser, "ekiumAdmin2022$");
			await userManager.AddToRoleAsync(newUser, "Ekium-Administrator");
		}

		#endregion

		#region Acts

		if (!anodeCTX.Acts.Any())
		{
			anodeCTX.Acts.Add(new Act { RID = ActionRID.ADMIN_GENERAL_RIGHTS });
			anodeCTX.SaveChanges();
		}

		#endregion

		#region ActEntities

		if (!anodeCTX.ActEntities.Any())
		{
			anodeCTX.ActEntities.Add(new ActEntity
			{
				RID = ActionRID.ADMIN_GENERAL_RIGHTS,
				ActID = anodeCTX.Acts.First(a => a.RID == ActionRID.ADMIN_GENERAL_RIGHTS).ID
			});
			anodeCTX.SaveChanges();
		}

		#endregion

		#region ActEntityRoles

		if (!anodeCTX.ActEntityRoles.Any())
		{
			anodeCTX.ActEntityRoles.Add(new ActEntityRole
			{
				RID = ActionRID.ADMIN_GENERAL_RIGHTS + "." + ApplicationRoleType.SYSTEM_EKIUM,
				ApplicationType = ApplicationTypeRID.ROLE,
				ApplicationID = anodeCTX.Roles.First(r => r.Type == ApplicationRoleType.SYSTEM_EKIUM).Id,
				ActEntityID = anodeCTX.ActEntities.First(ae => ae.RID == ActionRID.ADMIN_GENERAL_RIGHTS).ID
			});

			anodeCTX.ActEntityRoles.Add(new ActEntityRole
			{
				RID = ActionRID.ADMIN_GENERAL_RIGHTS + "." + ApplicationRoleType.SYSTEM_EKIDI,
				ApplicationType = ApplicationTypeRID.ROLE,
				ApplicationID = anodeCTX.Roles.First(r => r.Type == ApplicationRoleType.SYSTEM_EKIDI).Id,
				ActEntityID = anodeCTX.ActEntities.First(ae => ae.RID == ActionRID.ADMIN_GENERAL_RIGHTS).ID
			});
			anodeCTX.SaveChanges();
		}

		#endregion
	}
}