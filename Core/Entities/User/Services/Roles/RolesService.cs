using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Core.Entities.User.Models.DB.Roles;
using Core.Entities.User.Models.DTO.Roles;
using Core.Shared.Exceptions;
using Core.Shared.UnitOfWork.Interfaces;

namespace Core.Entities.User.Services.Roles
{
    public class RolesService : IRolesService
    {
        private readonly IAlarmUOW _alarmUOW;
        private readonly RoleManager<ApplicationRole> _roleManager;

        public RolesService(IAlarmUOW alarmUOW, RoleManager<ApplicationRole> roleManager)
        {
            _alarmUOW = alarmUOW;
			_roleManager = roleManager;
        }

        /// <summary>
        /// Get all roles
        /// </summary>
        /// <returns>A <see cref="List{DTORole}"/></returns>
        public async Task<List<DTORole>> GetAll()
        {
            return await _roleManager.Roles
                .Where(r => r.Type == "USER")
                .AsNoTracking()
                .Select(r => r.ToDTO())
                .ToListAsync();
        }

        /// <summary>
        /// Get a role by RID
        /// </summary>
        /// <param name="rid"></param>
        /// <returns>A <see cref="DTORole"/></returns>
        /// <exception cref="EntityNotFoundException"></exception>
        public async Task<DTORole> GetByRID(string rid)
		{
			ApplicationRole? role = await _roleManager.Roles
				.AsNoTracking()
				.FirstOrDefaultAsync(r => r.Name == rid);

			if (role == null)
				throw new EntityNotFoundException("role", rid);

			return role.ToDTO();
		}

        /// <summary>
        /// Create a new Role
        /// </summary>
        /// <param name="dtoRole"></param>
        /// <returns>The created <see cref="DTORole"/></returns>
        /// <exception cref="EntityNotFoundException"></exception>
        public async Task<DTORole> Create(DTORole dtoRole)
        {
            if (await _roleManager.RoleExistsAsync(dtoRole.RID))
                throw new EntityNotFoundException("Role RID already exist.");

            await _alarmUOW.StartTransaction();

            // Create Role with the common type USER
            ApplicationRole role = new(dtoRole.RID);

			IdentityResult result = await _roleManager.CreateAsync(role);
			if (!result.Succeeded)
				throw new EntityNotFoundException("An error happenened during the role creation.");

            await _alarmUOW.CommitTransaction();

            return role.ToDTO();
        }

		/// <summary>
		/// Update RoleLang which refers to Role id and Language RID
		/// </summary>
		/// <param name="rid"></param>
		/// <param name="dtoRole"></param>
		/// <returns>The updated <see cref="DTORole"/></returns>
		/// <exception cref="EntityNotFoundException"></exception>
		public async Task<DTORole> Update(string rid, DTORole dtoRole)
        {
            ApplicationRole? role = await _roleManager.Roles
                .FirstOrDefaultAsync(r => r.Name == rid);

            if (role == null)
                throw new EntityNotFoundException("Unable to find a role with RID {" + rid + "}.");

            _alarmUOW.Commit();

            return role.ToDTO();
        }

		/// <summary>
		/// Delete the Role and all RoleLangs attach to it
		/// </summary>
		/// <param name="rid"></param>
		/// <returns></returns>
		/// <exception cref="EntityNotFoundException"></exception>
		public async Task Delete(string rid)
        {
            var role = await _roleManager.Roles.FirstOrDefaultAsync(r => r.Name == rid);

            if (role == null)
                throw new EntityNotFoundException("Unable to find a role with RID {" + rid + "}.");

			await _roleManager.DeleteAsync(role);
        }
    }
}
