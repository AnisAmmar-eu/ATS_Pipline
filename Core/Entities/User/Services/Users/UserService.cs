﻿using System.DirectoryServices.AccountManagement;
using System.Runtime.Versioning;
using Core.Entities.User.Dictionaries;
using Core.Entities.User.Models.DB.Roles;
using Core.Entities.User.Models.DB.Users;
using Core.Entities.User.Models.DTO.Roles;
using Core.Entities.User.Models.DTO.Users;
using Core.Shared.Exceptions;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace Core.Entities.User.Services.Users;

public class UserService : IUserService
{
	private readonly IConfiguration _configuration;
	private readonly RoleManager<ApplicationRole> _roleManager;
	private readonly UserManager<ApplicationUser> _userManager;

	public UserService(
		UserManager<ApplicationUser> userManager,
		RoleManager<ApplicationRole> roleManager,
		IConfiguration configuration)
	{
		_userManager = userManager;
		_roleManager = roleManager;
		_configuration = configuration;
	}

	/// <summary>
	///     Get all users
	/// </summary>
	/// <returns>A <see cref="List{DTOUser}" /></returns>
	public async Task<List<DTOUser>> GetAll()
	{
		List<DTOUser> users = await _userManager.Users.Where(u => u.UserName != "ekium-admin")
			.AsNoTracking()
			.Select(u => u.ToDTO())
			.ToListAsync();
		// List<DTOUser> users = await _userManager.Users.Where(u => u.UserName != "ekium-admin" || u.UserName != "fives-admin")
		// 	.AsNoTracking()
		// 	.Select(u => u.ToDTO())
		// 	.ToListAsync();

		foreach (DTOUser user in users)
			user.Roles = await GetRolesByUsername(user.Username);

		return users;
	}

	/// <summary>
	///     Get a user by its username
	/// </summary>
	/// <param name="username"></param>
	/// <returns>A <see cref="DTOUser" /></returns>
	/// <exception cref="EntityNotFoundException"></exception>
	public async Task<DTOUser> GetByUsername(string username)
	{
		DTOUser? user = await _userManager.Users.Where(u => u.UserName == username)
			.AsNoTracking()
			.Select(u => u.ToDTO())
			.FirstOrDefaultAsync()
			?? throw new EntityNotFoundException("User [" + username + "] does not exist.");
		user.Roles = await GetRolesByUsername(user.Username);

		return user;
	}

	/// <summary>
	///     Get a user by ID
	/// </summary>
	/// <param name="id"></param>
	/// <returns>A <see cref="DTOUser" /></returns>
	/// <exception cref="EntityNotFoundException"></exception>
	public async Task<DTOUser> GetById(string id)
	{
		DTOUser? user = await _userManager.Users.Where(u => u.Id == id)
			.AsNoTracking()
			.Select(u => u.ToDTO())
			.FirstOrDefaultAsync()
			?? throw new EntityNotFoundException("User {" + id + "} does not exist.");
		user.Roles = await GetRolesByUsername(user.Username);

		return user;
	}

	/// <summary>
	///     Get a user ID by its username
	/// </summary>
	/// <param name="username"></param>
	/// <returns>A <see cref="string" /></returns>
	/// <exception cref="Exception"></exception>
	public async Task<string> GetIdByUsername(string username)
	{
		string? userId = await _userManager.Users
			.Where(u => u.UserName == username)
			.AsNoTracking()
			.Select(u => u.Id)
			.FirstOrDefaultAsync();

		return userId ?? throw new EntityNotFoundException("User [" + username + "] does not exist");
	}

	/// <summary>
	///     Get all roles of a user
	/// </summary>
	/// <param name="username"></param>
	/// <returns>A <see cref="List{DTORole}" /></returns>
	/// <exception cref="Exception"></exception>
	public async Task<List<DTORole>> GetRolesByUsername(string username)
	{
		ApplicationUser? user = await _userManager.Users
			.Where(u => u.UserName == username)
			.AsNoTracking()
			.FirstOrDefaultAsync()
			?? throw new EntityNotFoundException("User [" + username + "] does not exist");
		IList<string> rolesName = await _userManager.GetRolesAsync(user);

		return await _roleManager.Roles
			.Where(r => r.Name != null && rolesName.Contains(r.Name))
			.AsNoTracking()
			.Select(r => r.ToDTO())
			.ToListAsync();
	}

	/// <summary>
	///     Get all users with a specific role
	/// </summary>
	/// <param name="roleName"></param>
	/// <returns>An <see cref="List{ApplicationUser}" /></returns>
	public async Task<List<ApplicationUser>> GetAllByRole(string roleName)
		=> [.. (await _userManager.GetUsersInRoleAsync(roleName))];

	/// <summary>
	///     Update a user by its username
	/// </summary>
	/// <param name="username"></param>
	/// <param name="dtoUser"></param>
	/// <returns>The updated <see cref="DTOUser" /></returns>
	/// <exception cref="Exception"></exception>
	public async Task<DTOUser> Update(string username, DTOUser dtoUser)
	{
		ApplicationUser? user = await _userManager.Users.Where(u => u.UserName == username).FirstOrDefaultAsync()
			?? throw new EntityNotFoundException("User [" + username + "] does not exist");
		user.UserName = dtoUser.Username;
		user.Firstname = dtoUser.Firstname;
		user.Lastname = dtoUser.Lastname;
		//user.PhoneNumber = dtoUser.Phonenumber;

		await _userManager.UpdateAsync(user);

		// Get all rolesName
		IList<string> rolesName = await _userManager.GetRolesAsync(user);

		// // Remove admin role in rolesName
		// rolesName.Remove(RoleNames.Admin);

		// Remove all roles
		await _userManager.RemoveFromRolesAsync(user, rolesName);

		// Add new roles
		await _userManager.AddToRolesAsync(user, dtoUser.Roles.ConvertAll(r => r.RID));

		return dtoUser;
	}

	/// <summary>
	///     Set a user as admin
	/// </summary>
	/// <param name="username"></param>
	/// <param name="toAdmin"></param>
	/// <returns>True/False</returns>
	/// <exception cref="Exception"></exception>
	public async Task<bool> SetAdmin(string username, bool toAdmin)
	{
		ApplicationUser? user = await _userManager.Users
			.Where(u => u.UserName == username)
			.FirstOrDefaultAsync();

		return (user is null)
			? throw new EntityNotFoundException("User [" + username + "] does not exist")
			: toAdmin
			? (await _userManager.AddToRoleAsync(user, RoleNames.Admin)).Succeeded
			: (await _userManager.RemoveFromRoleAsync(user, RoleNames.Admin)).Succeeded;
	}

	/// <summary>
	///     Delete a user
	/// </summary>
	/// <param name="username"></param>
	/// <exception cref="Exception"></exception>
	public async Task Delete(string username)
	{
		ApplicationUser? user = await _userManager.Users
			.Where(r => r.UserName == username)
			.FirstOrDefaultAsync()
			?? throw new EntityNotFoundException("User [" + username + "] does not exist");
		await _userManager.DeleteAsync(user);
	}

	/// <summary>
	///     Change the user password using the username
	/// </summary>
	/// <param name="username"></param>
	/// <param name="newPassword"></param>
	/// <exception cref="Exception"></exception>
	public async Task UpdatePasswordByAdmin(string username, string newPassword)
	{
		ApplicationUser? user = await _userManager.Users
			.Where(u => u.UserName == username)
			.FirstOrDefaultAsync()
			?? throw new EntityNotFoundException("User [" + username + "] does not exist");
		string token = await _userManager.GeneratePasswordResetTokenAsync(user);

		await _userManager.ResetPasswordAsync(user, token, newPassword);
	}

	/// <summary>
	///     Change the user password using the name
	/// </summary>
	/// <param name="username"></param>
	/// <param name="oldPassword"></param>
	/// <param name="newPassword"></param>
	/// <exception cref="Exception"></exception>
	public async Task UpdatePasswordByUser(string username, string oldPassword, string newPassword)
	{
		ApplicationUser? user = await _userManager.Users
			.Where(u => u.UserName == username)
			.FirstOrDefaultAsync()
			?? throw new EntityNotFoundException("User [" + username + "] does not exist");
		IdentityResult result = await _userManager.ChangePasswordAsync(user, oldPassword, newPassword);

		if (!result.Succeeded)
			throw new("Current password is incorrect");
	}

	/// <summary>
	///     Reset the user password using the name
	/// </summary>
	/// <param name="name"></param>
	/// <exception cref="Exception"></exception>
	public async Task ResetPassword(string name)
	{
		ApplicationUser? user = await _userManager.Users
			.Where(u => u.UserName == name)
			.FirstOrDefaultAsync()
			?? throw new EntityNotFoundException("User does not exist");
		string token = await _userManager.GeneratePasswordResetTokenAsync(user);

		// Generate a random new password
		string newPassword = Guid.NewGuid().ToString()[..8];
		await _userManager.ResetPasswordAsync(user, token, newPassword);
	}

	/// <summary>
	///     Get all user from the Active Directory
	/// </summary>
	/// <returns>A <see cref="List{DTOUser}" /></returns>
	[SupportedOSPlatform("windows")]
	public List<DTOUser> GetAllFromAD()
	{
		PrincipalContext adContext = new(
			ContextType.Domain,
			_configuration["AdHost"],
			"OU=Bron,OU=Utilisateurs,OU=Ekium,DC=ekium,DC=lan");

		using PrincipalSearcher searcher = new(new UserPrincipal(adContext));

		return (from UserPrincipal user in searcher.FindAll().OrderBy(x => x.SamAccountName)
				select new DTOUser { Username = user.SamAccountName, Firstname = user.GivenName, Lastname = user.Surname })
			.ToList();
	}
}