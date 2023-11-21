using Core.Entities.User.Dictionaries;
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
				Name = RoleNames.ATS,
				NormalizedName = RoleNames.ATS.ToUpper(),
				Type = ApplicationRoleType.SYSTEM_ATS
			});
			anodeCTX.Roles.Add(new ApplicationRole
			{
				Name = RoleNames.FIVES,
				NormalizedName = RoleNames.FIVES.ToUpper(),
				Type = ApplicationRoleType.SYSTEM_FIVES
			});
			anodeCTX.Roles.Add(new ApplicationRole
			{
				Name = RoleNames.VISITOR,
				NormalizedName = RoleNames.VISITOR.ToUpper(),
				Type = ApplicationRoleType.USER
			});
			anodeCTX.Roles.Add(new ApplicationRole
			{
				Name = RoleNames.OPERATOR,
				NormalizedName = RoleNames.OPERATOR.ToUpper(),
				Type = ApplicationRoleType.USER
			});
			anodeCTX.Roles.Add(new ApplicationRole
			{
				Name = RoleNames.FORCING,
				NormalizedName = RoleNames.FORCING.ToUpper(),
				Type = ApplicationRoleType.USER
			});
			anodeCTX.Roles.Add(new ApplicationRole
			{
				Name = RoleNames.SETTINGS,
				NormalizedName = RoleNames.SETTINGS.ToUpper(),
				Type = ApplicationRoleType.USER
			});
			anodeCTX.Roles.Add(new ApplicationRole
			{
				Name = RoleNames.ADMIN,
				NormalizedName = RoleNames.ADMIN.ToUpper(),
				Type = ApplicationRoleType.USER
			});
			anodeCTX.SaveChanges();
		}

		#endregion

		#region Users

		if (!anodeCTX.Users.Any())
		{
			// Create User
			ApplicationUser ekiumUser = new()
			{
				UserName = "ekium-admin",
				Firstname = "ekium",
				Lastname = "admin",
				IsEkium = true,
				Source = SourceAuth.EKIDI
			};
			ApplicationUser fivesUser = new()
			{
				UserName = "fives-admin",
				Firstname = "fives",
				Lastname = "admin",
				IsEkium = true,
				Source = SourceAuth.EKIDI
			};

			await userManager.CreateAsync(ekiumUser, "ekiumAdmin2022$");
			await userManager.CreateAsync(fivesUser, "fivesAdmin2024$");

			// loop through the roles and add them to the user
			string[] roleNames = { RoleNames.ADMIN, RoleNames.ATS, RoleNames.FIVES, RoleNames.FORCING, RoleNames.OPERATOR, RoleNames.SETTINGS, RoleNames.VISITOR };
			foreach (string roleName in roleNames)
			{
				await userManager.AddToRoleAsync(ekiumUser, roleName);
				await userManager.AddToRoleAsync(fivesUser, roleName);
			}
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
				RID = ActionRID.ADMIN_GENERAL_RIGHTS + "." + ApplicationRoleType.SYSTEM_FIVES,
				ApplicationType = ApplicationTypeRID.ROLE,
				ApplicationID = anodeCTX.Roles.First(r => r.Type == ApplicationRoleType.SYSTEM_FIVES).Id,
				ActEntityID = anodeCTX.ActEntities.First(ae => ae.RID == ActionRID.ADMIN_GENERAL_RIGHTS).ID
			});

			anodeCTX.ActEntityRoles.Add(new ActEntityRole
			{
				RID = ActionRID.ADMIN_GENERAL_RIGHTS + "." + ApplicationRoleType.SYSTEM_ATS,
				ApplicationType = ApplicationTypeRID.ROLE,
				ApplicationID = anodeCTX.Roles.First(r => r.Type == ApplicationRoleType.SYSTEM_ATS).Id,
				ActEntityID = anodeCTX.ActEntities.First(ae => ae.RID == ActionRID.ADMIN_GENERAL_RIGHTS).ID
			});
			anodeCTX.SaveChanges();
		}

		#endregion
	}
}