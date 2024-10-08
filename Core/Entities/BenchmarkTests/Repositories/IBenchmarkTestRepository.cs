using Core.Entities.BenchmarkTests.Models.DB;
using Core.Entities.BenchmarkTests.Models.DTO;
using Core.Shared.Repositories.Kernel.Interfaces;

namespace Core.Entities.BenchmarkTests.Repositories;

public interface IBenchmarkTestRepository : IBaseEntityRepository<BenchmarkTest, DTOBenchmarkTest>
{
	public Task<List<BenchmarkTest>> OldGetRange(int start, int nbItems);
	public Task RemoveAll();
	public int GetCount();
}