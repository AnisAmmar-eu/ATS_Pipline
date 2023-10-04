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
	public static async Task Initialize(AlarmCTX alarmCTX, UserManager<ApplicationUser> userManager)
	{
		#region Roles

		if (!alarmCTX.Roles.Any())
		{
			alarmCTX.Roles.Add(new ApplicationRole
			{
				Name = "Ekidi-Administrator",
				NormalizedName = "EKIDI-ADMINISTRATOR",
				Type = ApplicationRoleType.SYSTEM_EKIDI
			});
			alarmCTX.Roles.Add(new ApplicationRole
			{
				Name = "Ekium-Administrator",
				NormalizedName = "EKIUM-ADMINISTRATOR",
				Type = ApplicationRoleType.SYSTEM_EKIUM
			});
			alarmCTX.SaveChanges();
		}

		#endregion

		#region Users

		if (!alarmCTX.Users.Any())
		{
			// Create User
			ApplicationUser newUser = new ApplicationUser
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

		if (!alarmCTX.Acts.Any())
		{
			alarmCTX.Acts.Add(new Act { RID = ActionRID.ADMIN_GENERAL_RIGHTS });
			alarmCTX.SaveChanges();
		}

		#endregion

		#region ActEntities

		if (!alarmCTX.ActEntities.Any())
		{
			alarmCTX.ActEntities.Add(new ActEntity
			{
				RID = ActionRID.ADMIN_GENERAL_RIGHTS,
				ActID = alarmCTX.Acts.First(a => a.RID == ActionRID.ADMIN_GENERAL_RIGHTS).ID
			});
			alarmCTX.SaveChanges();
		}

		#endregion

		#region ActEntityRoles

		if (!alarmCTX.ActEntityRoles.Any())
		{
			alarmCTX.ActEntityRoles.Add(new ActEntityRole
			{
				RID = ActionRID.ADMIN_GENERAL_RIGHTS + "." + ApplicationRoleType.SYSTEM_EKIUM,
				ApplicationType = ApplicationTypeRID.ROLE,
				ApplicationID = alarmCTX.Roles.First(r => r.Type == ApplicationRoleType.SYSTEM_EKIUM).Id,
				ActEntityID = alarmCTX.ActEntities.First(ae => ae.RID == ActionRID.ADMIN_GENERAL_RIGHTS).ID
			});

			alarmCTX.ActEntityRoles.Add(new ActEntityRole
			{
				RID = ActionRID.ADMIN_GENERAL_RIGHTS + "." + ApplicationRoleType.SYSTEM_EKIDI,
				ApplicationType = ApplicationTypeRID.ROLE,
				ApplicationID = alarmCTX.Roles.First(r => r.Type == ApplicationRoleType.SYSTEM_EKIDI).Id,
				ActEntityID = alarmCTX.ActEntities.First(ae => ae.RID == ActionRID.ADMIN_GENERAL_RIGHTS).ID
			});
			alarmCTX.SaveChanges();
		}

		#endregion
	}
}