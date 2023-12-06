using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Core.Entities.User.Dictionaries;
using Core.Entities.User.Models.DB.Acts;
using Core.Entities.User.Models.DB.Acts.ActEntities;
using Core.Entities.User.Models.DB.Acts.ActEntities.ActEntityRoles;
using Core.Entities.User.Models.DB.Roles;
using Core.Entities.User.Models.DB.Users;
using Core.Entities.User.Models.DTO.Acts;
using Core.Entities.User.Models.DTO.Acts.ActEntities;
using Core.Entities.User.Models.DTO.Acts.ActEntities.ActEntityRoles;
using Core.Entities.User.Repositories.Acts;
using Core.Entities.User.Services.Auth;
using Core.Shared.Exceptions;
using Core.Shared.Services.Jwt;
using Core.Shared.Services.Kernel;
using Core.Shared.UnitOfWork.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;

namespace Core.Entities.User.Services.Acts;

public class ActService : BaseEntityService<IActRepository, Act, DTOAct>, IActService
{
	private readonly IAuthService _authService;
	private readonly IJwtService _jwtService;
	private readonly RoleManager<ApplicationRole> _rolesManager;
	private readonly UserManager<ApplicationUser> _usersManager;

	public ActService(
		IAnodeUOW unitOfWork,
		IAuthService authService,
		IJwtService jwtService,
		UserManager<ApplicationUser> usersManager,
		RoleManager<ApplicationRole> rolesManager) : base(unitOfWork)
	{
		_authService = authService;
		_jwtService = jwtService;
		_usersManager = usersManager;
		_rolesManager = rolesManager;
	}

	/// <summary>
	///     Get all actions from a list and return them with their visibility status
	/// </summary>
	/// <param name="httpContext"></param>
	/// <param name="dtoActEntitiesStatus"></param>
	/// <returns>A <see cref="DTOActEntityStatus" /></returns>
	public async Task<List<DTOActEntityStatus>> ActionsFromList(
		HttpContext httpContext,
		List<DTOActEntityStatus> dtoActEntitiesStatus)
	{
		List<DTOActEntityStatus> dtoActEntities = new();

		foreach (DTOActEntityStatus dtoActEntityStatus in dtoActEntitiesStatus)
			dtoActEntities.Add(await GetAction(httpContext, dtoActEntityStatus));

		return dtoActEntities;
	}

	/// <summary>
	///     Get the visibility status of an action
	/// </summary>
	/// <param name="httpContext"></param>
	/// <param name="dtoActEntityStatus"></param>
	/// <returns>A <see cref="DTOActEntityStatus" /></returns>
	public async Task<DTOActEntityStatus> GetAction(HttpContext httpContext, DTOActEntityStatus dtoActEntityStatus)
	{
		Act act = await AnodeUOW.Acts.GetByRIDAndTypeWithIncludes(
			dtoActEntityStatus.Act?.RID,
			dtoActEntityStatus.Act?.EntityType,
			dtoActEntityStatus.Act?.ParentType);
		ActEntity actEntity;

		try
		{
			actEntity = await AnodeUOW.ActEntities
				.GetByActWithIncludes(act, dtoActEntityStatus.EntityID, dtoActEntityStatus.ParentID);
		}
		catch (Exception e)
		{
			if (e is not EntityNotFoundException)
				throw;

			dtoActEntityStatus.Visible = true;
			return dtoActEntityStatus;
		}

		dtoActEntityStatus.Act = act.ToDTO();
		dtoActEntityStatus.SignatureType = actEntity.SignatureType;

		if (httpContext.Items["UserRolesId"] is null || httpContext.Items["UserId"] is null)
			await _authService.SetContextWithUser(httpContext);

		string? userId = (string?)httpContext.Items["UserId"];
		List<string>? userRolesID = (List<string>?)httpContext.Items["UserRolesId"];

        if (httpContext.Items.ContainsKey("HasAdminRole") && (bool?)httpContext.Items["HasAdminRole"] == true)
        {
            dtoActEntityStatus.Visible = true;
        }
        else
        {
            switch (actEntity.SignatureType)
            {
                case SignatureTypeRID.Explicit:
                    dtoActEntityStatus.Visible = true;
                    break;
                case SignatureTypeRID.Session:
                    if (actEntity.ActEntityRoles.Count == 0)
                    {
                        dtoActEntityStatus.Visible = true;
                    }
                    else
                    {
                        dtoActEntityStatus.Visible = actEntity.ActEntityRoles.Any(aer =>
                            (aer.ApplicationType == ApplicationTypeRID.Role)
                                ? userRolesID?.Contains(aer.ApplicationID) == true
                                : aer.ApplicationType == ApplicationTypeRID.User && aer.ApplicationID == userId
                                                );
                    }

                    break;
                default:
                    dtoActEntityStatus.Visible = true;
                    break;
            }
        }

        return dtoActEntityStatus;
	}

	/// <summary>
	///     Get all actEntityRoles of an actEntity
	/// </summary>
	/// <param name="dtoActEntity"></param>
	/// <returns>A <see cref="DTOActEntity" /></returns>
	public async Task<DTOActEntity> GetActionEntityRoles(DTOActEntity dtoActEntity)
	{
		Act act = await AnodeUOW.Acts
			.GetByRIDAndTypeWithIncludes( dtoActEntity.Act?.RID, dtoActEntity.Act?.EntityType, dtoActEntity.Act?.ParentType);
		ActEntity actEntity;

		try
		{
			actEntity = await AnodeUOW.ActEntities.GetByActWithIncludes( act, dtoActEntity.EntityID, dtoActEntity.ParentID);
		}
		catch (Exception e)
		{
			if (e is not EntityNotFoundException)
				throw;

			dtoActEntity.Act = act.ToDTO();
			return dtoActEntity;
		}

		dtoActEntity = actEntity.ToDTO();

		foreach (ActEntityRole actEntityRole in actEntity.ActEntityRoles)
		{
			string? applicationName = actEntityRole.ApplicationType switch {
				ApplicationTypeRID.Role => (await _rolesManager.FindByIdAsync(actEntityRole.ApplicationID))?.Name,
				ApplicationTypeRID.User => (await _usersManager.FindByIdAsync(actEntityRole.ApplicationID))?.UserName,
				_ => null,
			};

			dtoActEntity.Applications.Add(actEntityRole.ToDTO(applicationName));
		}

		return dtoActEntity;
	}

	public async Task AssignAction(DTOActEntity dtoActEntity, bool remove = true)
	{
		Act act = await AnodeUOW.Acts
			.GetByRIDAndTypeWithIncludes(dtoActEntity.Act?.RID, dtoActEntity.Act?.EntityType, dtoActEntity.Act?.ParentType);
		string? signatureType = dtoActEntity.SignatureType;

		ActEntity? actEntity = null;

		try
		{
			actEntity = await AnodeUOW.ActEntities.GetByActWithIncludes(act, dtoActEntity.EntityID, dtoActEntity.ParentID);
		}
		catch (Exception e)
		{
			if (e is not EntityNotFoundException)
				throw;
		}

		await AnodeUOW.StartTransaction();

		if (actEntity is null)
		{
			if (signatureType is null)
			{
				await AnodeUOW.CommitTransaction();
				return;
			}

			actEntity = dtoActEntity.ToModel(act, signatureType);

			await AnodeUOW.ActEntities.Add(actEntity);
		}
		else
		{
			if (signatureType is null)
			{
				AnodeUOW.ActEntities.Remove(actEntity);
				AnodeUOW.Commit();
				await AnodeUOW.CommitTransaction();
				return;
			}

			actEntity.SignatureType = signatureType;

			if (remove)
				actEntity.ActEntityRoles.Clear();

			AnodeUOW.ActEntities.Update(actEntity);
		}

		AnodeUOW.Commit();

		switch (signatureType)
		{
			case SignatureTypeRID.Session:
			case SignatureTypeRID.Explicit:
				if (dtoActEntity.Applications.Any(a => a.Type == ApplicationTypeRID.Role))
                {
                    foreach (DTOActEntityRole application in dtoActEntity.Applications.FindAll(a =>
                        a.Type == ApplicationTypeRID.Role))
					{
						ApplicationRole? role = await _rolesManager.FindByNameAsync(application.Name ?? string.Empty);
						if (role is null)
							throw new EntityNotFoundException("role", application.Name ?? string.Empty);

						actEntity.ActEntityRoles?.Add(new ActEntityRole(actEntity, role));
					}
                }

                if (dtoActEntity.Applications.Any(a => a.Type == ApplicationTypeRID.User))
                {
                    foreach (DTOActEntityRole application in dtoActEntity.Applications.FindAll(a =>
                        a.Type == ApplicationTypeRID.User))
					{
						ApplicationUser? user = await _usersManager.FindByNameAsync(application.Name ?? string.Empty);
						if (user is null)
							throw new EntityNotFoundException("user", application.Name ?? string.Empty);

						actEntity.ActEntityRoles?.Add(new ActEntityRole(actEntity, user));
					}
                }

                break;
		}

		AnodeUOW.ActEntities.Update(actEntity);
		AnodeUOW.Commit();

		await AnodeUOW.CommitTransaction();
	}

    /// <summary>
    ///     Delete an actionEntities based on an act and some ids
    /// </summary>
    /// <param name="actRID"></param>
    /// <param name="entityType"></param>
    /// <param name="entityID"></param>
    /// <param name="parentType"></param>
    /// <param name="parentID" />
    /// <param name="parentID"></param>
    public async Task DeleteActionEntity(
        string actRID,
        string? entityType = null,
        int? entityID = null,
        string? parentType = null,
        int? parentID = null)
    {
        Act act = await AnodeUOW.Acts.GetByRIDAndTypeWithIncludes(actRID, entityType, parentType);

        // [T0DO] -> Confirm if the list is necessary 
        List<ActEntity> actEntities = await AnodeUOW.ActEntities.GetAllByActWithIncludes(act, entityID, parentID);

        if (actEntities.Count == 0)
            return;

        AnodeUOW.ActEntities.RemoveRange(actEntities);
        AnodeUOW.Commit();
    }

    /// <summary>
    ///     Duplicate actionEntities
    /// </summary>
    /// <param name="dtoActToDuplicate"></param>
    /// <param name="dtoAct"></param>
    /// <returns>True/False</returns>
    public async Task<bool> DuplicateActionEntities(DTOActEntityToValid dtoActToDuplicate, DTOActEntityToValid dtoAct)
	{
		Act actFromDuplicate = await AnodeUOW.Acts.GetByRIDAndTypeWithIncludes(
			dtoActToDuplicate.Act?.RID,
			dtoActToDuplicate.Act?.EntityType,
			dtoActToDuplicate.Act?.ParentType);
		Act act = await AnodeUOW.Acts
			.GetByRIDAndTypeWithIncludes(dtoAct.Act?.RID, dtoAct.Act?.EntityType, dtoAct.Act?.ParentType);

		ActEntity? actEntityToDuplicate;

		try
		{
			actEntityToDuplicate = await AnodeUOW.ActEntities
				.GetByActWithIncludes(actFromDuplicate, dtoActToDuplicate.EntityID, dtoActToDuplicate.ParentID);
		}
		catch (Exception e)
		{
			if (e is not EntityNotFoundException)
				throw;

			return false;
		}

		await AnodeUOW.StartTransaction();

		ActEntity? actEntity;

		try
		{
			actEntity = await AnodeUOW.ActEntities.GetByActWithIncludes(act, dtoAct.EntityID, dtoAct.ParentID);
		}
		catch (Exception e)
		{
			if (e is not EntityNotFoundException)
				throw;

			actEntity = new()
            {
             RID = act.RID + "." + dtoAct.EntityID,
             Act = act,
             EntityID = dtoAct.EntityID,
             ParentID = dtoAct.ParentID,
             SignatureType = actEntityToDuplicate.SignatureType,
			};

			await AnodeUOW.ActEntities.Add(actEntity);
			AnodeUOW.Commit();
		}

		foreach (ActEntityRole actEntityRole in actEntityToDuplicate.ActEntityRoles)
        {
            actEntity.ActEntityRoles.Add(new ActEntityRole {
                ActEntity = actEntity,
                ApplicationID = actEntityRole.ApplicationID,
                ApplicationType = actEntityRole.ApplicationType,
                });
        }

        AnodeUOW.ActEntities.Update(actEntity);
		AnodeUOW.Commit();

		await AnodeUOW.CommitTransaction();

		return true;
	}

	/// <summary>
	///     Check the rights to do action with entity in order to generate a token
	/// </summary>
	/// <param name="httpContext"></param>
	/// <param name="dtoActEntityToValid"></param>
	/// <returns>The action token as a string</returns>
	/// <exception cref="UnauthorizedAccessException"></exception>
	public async Task<string> HasRights(HttpContext httpContext, DTOActEntityToValid dtoActEntityToValid)
	{
		await _authService.SetContextWithUser(httpContext);

		Act act = await AnodeUOW.Acts.GetByRIDAndTypeWithIncludes(
			dtoActEntityToValid.Act?.RID,
			dtoActEntityToValid.Act?.EntityType,
			dtoActEntityToValid.Act?.ParentType);

		ActEntity actEntity;

		try
		{
			actEntity = await AnodeUOW.ActEntities
				.GetByActWithIncludes(act, dtoActEntityToValid.EntityID, dtoActEntityToValid.ParentID);
		}
		catch (Exception e)
		{
			if (e is not EntityNotFoundException)
				throw;

			// Return a token if there is no actEntity - because authorized to all users
			// [T0DO] -> Maybe also verif if entityID / parentID exist so that we don't generate a token for an entity that does not exist

			return GenerateActionToken(
				httpContext,
				new ActEntity { Act = act, EntityID = dtoActEntityToValid.EntityID, ParentID = dtoActEntityToValid.ParentID });
		}

		// If an actEntity exist -> check authorization by signatureRID
		// By default authorized set to false
		bool authorized = false;
		string? userId = (string?)httpContext.Items["UserId"];
		List<string>? userRolesID = (List<string>?)httpContext.Items["UserRolesId"];

        if (httpContext.Items.ContainsKey("HasAdminRole") && (bool?)httpContext.Items["HasAdminRole"] == true)
        {
            authorized = true;
        }
        else
        {
            switch (actEntity.SignatureType)
            {
                case SignatureTypeRID.Session:
                    if (actEntity.ActEntityRoles.Count == 0)
                    {
                        authorized = true;
                    }
                    else
                    {
                        authorized = actEntity.ActEntityRoles.Any(aer =>
                            (aer.ApplicationType == ApplicationTypeRID.Role)
                                ? userRolesID?.Contains(aer.ApplicationID) == true
                                : aer.ApplicationType == ApplicationTypeRID.User && aer.ApplicationID == userId);
                    }

                    break;
                case SignatureTypeRID.Explicit:
                    // Check credentials inside DTOAct -> role match with action entity roles
                    if (dtoActEntityToValid.Login is not null)
                    {
                        ApplicationUser otherUser = await _authService.CheckCredentials(dtoActEntityToValid.Login);
                        List<string> rolesId = await _authService.GetRolesIdFromUser(otherUser, httpContext);

                        if (httpContext.Items.ContainsKey("HasAdminRole")
                            && (bool?)httpContext.Items["HasAdminRole"] == true)
                        {
                            authorized = true;
                        }
                        else
                        {
                            authorized = actEntity.ActEntityRoles.Any(aer =>
                                (rolesId.Contains(aer.ApplicationID)
                                    && aer.ApplicationType == ApplicationTypeRID.Role)
                                    || (aer.ApplicationID == otherUser.Id
                                        && aer.ApplicationType == ApplicationTypeRID.User));
                        }
                    }

                    break;
            }
        }

        if (authorized)
        {
            // Generate action token if the check is OK
            return GenerateActionToken(httpContext, actEntity);
        }

        throw new UnauthorizedAccessException("Unauthorized to execute this action");
	}

	/// <summary>
	///     To call in each method like GET operation, ...
	/// </summary>
	/// <param name="httpContext"></param>
	/// <param name="dtoActEntitiesToValid"></param>
	/// <returns>True/False</returns>
	public bool ValidActionToken(HttpContext httpContext, List<DTOActEntityToValid> dtoActEntitiesToValid)
	{
		string? actionToken = httpContext.Request.Headers["x-action-token"];

		if (actionToken is null)
			return false;

		JwtSecurityToken actionTokenData = _jwtService.ValidToken(actionToken);

		// Check if actionToken has been generated by the current User
		if (httpContext.User.Claims.Where(x => x.Type == "Id").Select(c => c.Value).FirstOrDefault() != actionTokenData
			.Claims
			.Where(x => x.Type == "UserId")
			.Select(c => c.Value)
			.FirstOrDefault())
        {
            return false;
        }

        int count = 0;
		foreach (DTOActEntityToValid dtoActEntityToValid in dtoActEntitiesToValid)
		{
			if (actionTokenData.Claims.Where(x => x.Type == "ActionRID").Select(c => c.Value).FirstOrDefault()
			    == dtoActEntityToValid.Act?.RID
				&& (dtoActEntityToValid.EntityID is null
					|| actionTokenData.Claims.Where(x => x.Type == "EntityId").Select(c => c.Value).FirstOrDefault()
					== dtoActEntityToValid.EntityID.ToString())
				&& (dtoActEntityToValid.ParentID is null
					|| actionTokenData.Claims.Where(x => x.Type == "ParentId").Select(c => c.Value).FirstOrDefault()
					== dtoActEntityToValid.ParentID.ToString())
				&& (dtoActEntityToValid.Act?.EntityType is null
					|| actionTokenData.Claims.Where(x => x.Type == "EntityType").Select(c => c.Value).FirstOrDefault()
					== dtoActEntityToValid.Act?.EntityType)
				&& (dtoActEntityToValid.Act?.ParentType is null
					|| actionTokenData.Claims.Where(x => x.Type == "ParentType").Select(c => c.Value).FirstOrDefault()
					== dtoActEntityToValid.Act?.ParentType)
				&& actionTokenData.Claims.Where(x => x.Type == "Status").Select(c => c.Value).FirstOrDefault()
					== "Complete")
            {
                break;
            }

            count++;
		}

		return count < dtoActEntitiesToValid.Count;
	}

	private string GenerateActionToken(HttpContext httpContext, ActEntity actEntity)
	{
		// Set claims list to add in the token
		List<Claim> claims = [
			new("UserId", httpContext.Items["UserId"]?.ToString() ?? string.Empty),
			new("ActionRID", actEntity.Act.RID),
			new("Status", "Complete"),
			new(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
		];
		if (actEntity.EntityID is not null)
			claims.Add(new Claim("EntityId", actEntity.EntityID.ToString() ?? string.Empty));

		if (actEntity.ParentID is not null)
			claims.Add(new Claim("ParentId", actEntity.ParentID.ToString() ?? string.Empty));

		if (actEntity.Act.EntityType is not null)
			claims.Add(new Claim("EntityType", actEntity.Act.EntityType));

		if (actEntity.Act.ParentType is not null)
			claims.Add(new Claim("ParentType", actEntity.Act.ParentType));

		return _jwtService.GenerateToken(claims, 2);
	}

	// [T0DO] [FOR SIGNAL_R]
	// => Check all users and roles that can do the actEntities
	// => If a user has one of the roles, then remove him
	// => Return a strings table of all groups name to send notif with signalR
	// public async Task<List<string>> GetGroupsToSend(List<DTOActEntityToValid> dtoActEntitiesToValid)
	// {
	//     List<string> groups = new List<string>{};

	//     // Check that we have actions
	//     if (dtoActEntitiesToValid.Count <= 0 || dtoActEntitiesToValid is null) 
	//     {
	//         throw new EntityNotFoundException("No actions specified.");
	//     }

	//     ActEntity actEntity = null;
	//     DTOActEntityToValid lastDtoActEntityToValid = null;
	//     int count = 0;

	//     // Foreach dtoActEntityToValid, check if exist, else count += 1
	//     foreach (DTOActEntityToValid dtoActEntityToValid in dtoActEntitiesToValid.OrderBy(dto => dto.Priority))
	//     {
	//         lastDtoActEntityToValid = dtoActEntityToValid;

	//         actEntity = await _context.ActEntities
	//             .Include(ae => ae.SignatureType)
	//             .Include(ae => ae.Act)
	//             .Include(ae => ae.ActEntityRoles)
	//             .FirstOrDefaultAsync(ae => ae.Act.RID == dtoActEntityToValid.ActionRID 
	//                 && ae.EntityID == dtoActEntityToValid.EntityID && ae.ParentID == dtoActEntityToValid.ParentID
	//                 && ae.Act.EntityType == dtoActEntityToValid.EntityType && ae.Act.ParentType == dtoActEntityToValid.ParentType
	//             );

	//         if (actEntity != null)
	//             break;

	//         count++;
	//     }

	//     if (actEntity != null && count < dtoActEntitiesToValid.Count)
	//     {
	//         List<string> users = new List<string>{};
	//         List<string> roles = new List<string>{};

	//         foreach (ActEntityRole actEntityRole in actEntity.ActEntityRoles)
	//         {
	//             // To check
	//             if (actEntityRole.ApplicationType == ApplicationTypeRID.ROLE)
	//             {
	//                 var role = await _context.Roles.FirstOrDefaultAsync(r => r.Id == actEntityRole.ApplicationID);
	//                 roles.Add(role.Name);
	//             } else if (actEntityRole.ApplicationType == ApplicationTypeRID.USER)
	//             {
	//                 var user = await _context.Users.FirstOrDefaultAsync(u => u.Id == actEntityRole.ApplicationID);
	//                 users.Add(user.UserName);
	//             }
	//         }

	//         // Check if  a user has a one of the role -> remove him
	//         foreach (string username in users.ToList())
	//         {
	//             var user = await _context.Users.FirstOrDefaultAsync(u => u.UserName == username);
	//             var userRoles = await _authService.GetRolesAsync(user);

	//             if (roles.Intersect(userRoles).Count() > 0) {
	//                 users.Remove(username);
	//             }
	//         }

	//         groups.AddRange(roles.Concat(users));
	//     }

	//     return groups;
	// }
}