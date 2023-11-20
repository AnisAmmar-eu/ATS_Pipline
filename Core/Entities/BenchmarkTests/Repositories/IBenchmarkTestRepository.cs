using Core.Entities.BenchmarkTests.Models.DB;
using Core.Entities.BenchmarkTests.Models.DTO;
using Core.Shared.Pagination;
using Core.Shared.Repositories.Kernel.Interfaces;

namespace Core.Entities.BenchmarkTests.Repositories;

public interface IBenchmarkTestRepository : IRepositoryBaseEntity<BenchmarkTest, DTOBenchmarkTest>
{
	public Task<List<BenchmarkTest>> OldGetRange(int start, int nbItems);
	public Task<List<BenchmarkTest>> GetRangeForPagination(int nbItems, int lastID, Pagination pagination);
	public Task RemoveAll();
	public int GetCount();
}