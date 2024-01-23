using Core.Entities.User.Dictionaries;
using Core.Entities.User.Models.DB.Acts;
using Core.Entities.User.Models.DB.Acts.ActEntities;
using Core.Entities.User.Models.DB.Acts.ActEntities.ActEntityRoles;
using Core.Entities.User.Models.DB.Roles;
using Core.Entities.User.Models.DB.Users;
using Core.Shared.Data;
using Microsoft.AspNetCore.Identity;

namespace Core.Entities.User.Data;

public static class UserInitializer
{
	public static async Task Initialize(AnodeCTX anodeCTX, UserManager<ApplicationUser> userManager)
	{
		#region Roles

		if (!anodeCTX.Roles.Any())
		{
			anodeCTX.Roles.Add(new ApplicationRole
				{
					Name = RoleNames.Operator,
					NormalizedName = RoleNames.Operator.ToUpper(),
					Type = ApplicationRoleType.User,
				});
			anodeCTX.Roles.Add(new ApplicationRole
				{
					Name = RoleNames.Supervisor,
					NormalizedName = RoleNames.Supervisor.ToUpper(),
					Type = ApplicationRoleType.User,
				});
			anodeCTX.Roles.Add(new ApplicationRole
				{
					Name = RoleNames.Maintenance,
					NormalizedName = RoleNames.Maintenance.ToUpper(),
					Type = ApplicationRoleType.User,
				});
			anodeCTX.Roles.Add(new ApplicationRole
				{
					Name = RoleNames.Engineer,
					NormalizedName = RoleNames.Engineer.ToUpper(),
					Type = ApplicationRoleType.User,
				});
			anodeCTX.Roles.Add(new ApplicationRole
				{
					Name = RoleNames.Admin,
					NormalizedName = RoleNames.Admin.ToUpper(),
					Type = ApplicationRoleType.User,
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
					Source = SourceAuth.Ekidi,
				};
			ApplicationUser fivesUser = new()
				{
					UserName = "fives-admin",
					Firstname = "fives",
					Lastname = "admin",
					IsEkium = true,
					Source = SourceAuth.Ekidi,
				};

			await userManager.CreateAsync( ekiumUser, "ekiumAdmin2022$");
			await userManager.CreateAsync(fivesUser, "fivesAdmin2024$");

			// loop through the roles and add them to the user
			string[] roleNames = [
					RoleNames.Admin, RoleNames.Maintenance, RoleNames.Operator,
					RoleNames.Engineer, RoleNames.Supervisor,
				];
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
			anodeCTX.Acts.Add(new Act { RID = ActionRID.AdminGeneralRights });
			anodeCTX.SaveChanges();
		}

		#endregion

		#region ActEntities

		if (!anodeCTX.ActEntities.Any())
		{
			anodeCTX.ActEntities.Add(new ActEntity
				{
					RID = ActionRID.AdminGeneralRights,
					ActID = anodeCTX.Acts.First(a => a.RID == ActionRID.AdminGeneralRights).ID,
				});
			anodeCTX.SaveChanges();
		}

		#endregion

		#region ActEntityRoles

		if (anodeCTX.ActEntityRoles.Any())
			return;

		anodeCTX.ActEntityRoles.Add(new ActEntityRole
			{
				RID = ActionRID.AdminGeneralRights + "." + ApplicationRoleType.SystemFives,
				ApplicationType = ApplicationTypeRID.Role,
				ApplicationID = anodeCTX.Roles.First(r => r.Type == ApplicationRoleType.User).Id,
				ActEntityID = anodeCTX.ActEntities.First(ae => ae.RID == ActionRID.AdminGeneralRights).ID,
			});

		anodeCTX.ActEntityRoles.Add(new ActEntityRole
			{
				RID = ActionRID.AdminGeneralRights + "." + ApplicationRoleType.SystemATS,
				ApplicationType = ApplicationTypeRID.Role,
				ApplicationID = anodeCTX.Roles.First(r => r.Type == ApplicationRoleType.User).Id,
				ActEntityID = anodeCTX.ActEntities.First(ae => ae.RID == ActionRID.AdminGeneralRights).ID,
			});
		anodeCTX.SaveChanges();

		#endregion
	}
}