using Core.Entities.User.Models.DTO.Roles;

namespace Core.Entities.User.Services.Roles;

public interface IRoleService
{
	Task<List<DTORole>> GetAll();
	Task<DTORole> GetByRID(string rid);
	Task<DTORole> Create(DTORole dtoRole);
	Task<DTORole> Update(string rid, DTORole dtoRole);
	Task Delete(string rid);
}